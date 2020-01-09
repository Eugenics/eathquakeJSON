using System;
using System.Text.Json;
using System.Net;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;

namespace eathquakeJSON
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Insert date from in format - YYYY-MM-DD");
            //string dateFrom = DateTime.Today.AddDays(-1).ToString();
            string dateFrom = Console.ReadLine();
            Console.WriteLine(dateFrom);
            Console.WriteLine("Insert date till in format - YYYY-MM-DD");
            //string dateTill = DateTime.Today.AddDays(1).ToString();
            string dateTill = Console.ReadLine();
            Console.WriteLine(dateTill);

            List<IEathquake> eathquakes = getJSON(@"https://earthquake.usgs.gov/fdsnws/event/1/query?format=geojson&starttime=" + dateFrom + "&endtime=" + dateTill + "&minmagnitude=2");

            addToDB(eathquakes);
        }

        static List<IEathquake> getJSON(string url)
        {
            string jsonString;
            List<IEathquake> eathquakes = new List<IEathquake>();

            StreamWriter ioLogFile = new StreamWriter(Directory.GetCurrentDirectory() +  @"\log\web_log_"
                + DateTime.Now.ToLocalTime().ToString().Replace(':', '_').Replace(' ', '_')
                + ".log");

            using (WebClient webClient = new WebClient())
            {
                try
                {
                    ioLogFile.WriteLine(DateTime.Now.ToString());
                    ioLogFile.WriteLine(url);
                    ioLogFile.WriteLine();

                    jsonString = webClient.DownloadString(url);
                    
                    ioLogFile.WriteLine(jsonString);
                    ioLogFile.WriteLine();
                }
                catch (WebException ex)
                {
                    ioLogFile.WriteLine(ex.Message);
                    return null;
                }
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            using (var jsonDocument = JsonDocument.Parse(jsonString))
            {
                var rootElem = jsonDocument.RootElement;
                var features = rootElem.GetProperty("features");

                foreach (var feature in features.EnumerateArray())
                {
                    IEathquake eathquake = new Eathquake();

                    var featureProp = feature.GetProperty("properties");

                    eathquake.id = feature.GetProperty("id").ToString();
                    eathquake.mag = Convert.ToDecimal(featureProp.GetProperty("mag").ToString().Replace('.', ','));
                    eathquake.place = featureProp.GetProperty("place").ToString();
                    eathquake.time = Convert.ToInt64(featureProp.GetProperty("time").ToString());
                    eathquake.updated = Convert.ToInt64(featureProp.GetProperty("updated").ToString());
                    eathquake.url = featureProp.GetProperty("url").ToString();
                    eathquake.detailUrl = featureProp.GetProperty("detail").ToString();
                    eathquake.tsunami = Convert.ToInt32(featureProp.GetProperty("tsunami").ToString());
                    eathquake.sig = Convert.ToInt32(featureProp.GetProperty("sig").ToString());
                    eathquake.net = featureProp.GetProperty("net").ToString();
                    eathquake.code = featureProp.GetProperty("code").ToString();
                    eathquake.ids = featureProp.GetProperty("ids").ToString();
                    eathquake.sources = featureProp.GetProperty("sources").ToString();
                    eathquake.types = featureProp.GetProperty("types").ToString();
                    eathquake.rms = Convert.ToDecimal(featureProp.GetProperty("rms").ToString().Replace('.', ','));
                    eathquake.magType = featureProp.GetProperty("magType").ToString();
                    eathquake.type = featureProp.GetProperty("type").ToString();
                    eathquake.title = featureProp.GetProperty("title").ToString();
                    eathquake.tz = Convert.ToInt32(featureProp.GetProperty("tz").ToString());
                    eathquake.felt = featureProp.GetProperty("felt").ToString() != "" ? Convert.ToInt32(featureProp.GetProperty("felt").ToString()) : 0;
                    eathquake.cdi = featureProp.GetProperty("cdi").ToString() != "" ? Convert.ToDecimal(featureProp.GetProperty("cdi").ToString().Replace('.', ',')) : 0;
                    eathquake.mmi = featureProp.GetProperty("mmi").ToString() != "" ? Convert.ToDecimal(featureProp.GetProperty("mmi").ToString().Replace('.', ',')) : 0;
                    eathquake.alert = featureProp.GetProperty("alert").ToString();
                    eathquake.status = featureProp.GetProperty("status").ToString();
                    eathquake.nst = featureProp.GetProperty("nst").ToString() != "" ? Convert.ToInt32(featureProp.GetProperty("nst").ToString()) : 0;
                    eathquake.dmin = featureProp.GetProperty("dmin").ToString() != "" ? Convert.ToDecimal(featureProp.GetProperty("dmin").ToString().Replace('.', ',')) : 0;
                    eathquake.gap = featureProp.GetProperty("gap").ToString() != "" ? Convert.ToDecimal(featureProp.GetProperty("gap").ToString().Replace('.', ',')) : 0;


                    var coordinates = feature.GetProperty("geometry").GetProperty("coordinates").ToString()
                        .Replace('[', ' ')
                        .Replace(']', ' ')
                        .Trim();

                    string[] coordArr = coordinates.Split(',');

                    if (coordArr.Length == 3)
                    {
                        IEathquakeGeometry geometry = new EathquakeGeometry();
                        geometry.latitude = Convert.ToDecimal(coordArr[1].Replace('.', ','));
                        geometry.longitude = Convert.ToDecimal(coordArr[0].Replace('.', ','));
                        geometry.depth = Convert.ToDecimal(coordArr[2].Replace('.', ','));

                        eathquake.eathquakeGeometry = geometry;
                    }


                    eathquakes.Add(eathquake);
                }
            }

            return eathquakes;
        }


        static void addToDB(List<IEathquake> eathquakes)
        {

            string serverName = "172.22.1.11";
            string dbName = "vizir_sfo";
            SqlConnection conn = new SqlConnection("Data Source=" + serverName + ";Initial Catalog=" + dbName + ";UID=podzorov;PWD=Tdutybrf@2020;");

            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = "viz2.insert_eathquake_data_from_json";
            cmd.Connection = conn;

            SqlParameter srcPar = new SqlParameter();
            SqlParameter idEathquakePar = new SqlParameter();
            SqlParameter versionPar = new SqlParameter();
            SqlParameter eathquakeTimePar = new SqlParameter();
            SqlParameter lathPar = new SqlParameter();
            SqlParameter longPar = new SqlParameter();
            SqlParameter magPar = new SqlParameter();
            SqlParameter depthPat = new SqlParameter();
            SqlParameter nstPar = new SqlParameter();
            SqlParameter regionPar = new SqlParameter();
            SqlParameter dataSourcePar = new SqlParameter();
            SqlParameter createTimePar = new SqlParameter();

            srcPar.ParameterName = "SRC";
            idEathquakePar.ParameterName = "ID_EARTHQUAKE";
            versionPar.ParameterName = "VERSION";
            eathquakeTimePar.ParameterName = "EARTHQUAKE_TIME";
            lathPar.ParameterName = "LAT";
            longPar.ParameterName = "LNG";
            magPar.ParameterName = "MAG";
            depthPat.ParameterName = "DEPTH";
            nstPar.ParameterName = "NST";
            regionPar.ParameterName = "REGION";
            dataSourcePar.ParameterName = "DATA_SOURCE";
            createTimePar.ParameterName = "CREATE_DATE";

            srcPar.DbType = System.Data.DbType.String;
            idEathquakePar.DbType = System.Data.DbType.String;
            versionPar.DbType = System.Data.DbType.String;
            eathquakeTimePar.DbType = System.Data.DbType.DateTime;
            lathPar.DbType = System.Data.DbType.Decimal;
            longPar.DbType = System.Data.DbType.Decimal;
            magPar.DbType = System.Data.DbType.Decimal;
            depthPat.DbType = System.Data.DbType.Int64;
            nstPar.DbType = System.Data.DbType.String;
            regionPar.DbType = System.Data.DbType.String;
            dataSourcePar.DbType = System.Data.DbType.String;
            createTimePar.DbType = System.Data.DbType.DateTime;

            cmd.Parameters.Add(srcPar);
            cmd.Parameters.Add(idEathquakePar);
            cmd.Parameters.Add(versionPar);
            cmd.Parameters.Add(eathquakeTimePar);
            cmd.Parameters.Add(lathPar);
            cmd.Parameters.Add(longPar);
            cmd.Parameters.Add(magPar);
            cmd.Parameters.Add(depthPat);
            cmd.Parameters.Add(nstPar);
            cmd.Parameters.Add(regionPar);
            cmd.Parameters.Add(dataSourcePar);
            cmd.Parameters.Add(createTimePar);


            StreamWriter ioLogFile = new StreamWriter(Directory.GetCurrentDirectory() + @"\log\sql_log_"
                + DateTime.Now.ToLocalTime().ToString().Replace(':', '_').Replace(' ', '_')
                + ".log");

            foreach (IEathquake eathquake in eathquakes)
            {
                cmd.Parameters[0].Value = "usgc";
                cmd.Parameters[1].Value = eathquake.id;
                cmd.Parameters[2].Value = "1";

                DateTime eathquakeDT = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);

                cmd.Parameters[3].Value = eathquakeDT.AddMilliseconds(eathquake.time).ToLocalTime();
                cmd.Parameters[4].Value = eathquake.eathquakeGeometry.latitude;
                cmd.Parameters[5].Value = eathquake.eathquakeGeometry.longitude;
                cmd.Parameters[6].Value = eathquake.mag;
                cmd.Parameters[7].Value = eathquake.eathquakeGeometry.depth;
                cmd.Parameters[8].Value = eathquake.nst;
                cmd.Parameters[9].Value = eathquake.title;
                cmd.Parameters[10].Value = "usgs-gov";
                cmd.Parameters[11].Value = eathquakeDT.AddMilliseconds(eathquake.updated).ToLocalTime();

                try
                {
                    conn.Open();

                    ioLogFile.WriteLine(DateTime.Now.ToString());
                    ioLogFile.WriteLine(serverName);
                    ioLogFile.WriteLine(dbName);
                    ioLogFile.WriteLine(cmd.CommandText);
                    ioLogFile.WriteLine(
                        cmd.Parameters[0].Value.ToString() + "; "
                        + cmd.Parameters[1].Value.ToString() + "; "
                        + cmd.Parameters[2].Value.ToString() + "; "
                        + cmd.Parameters[3].Value.ToString() + "; "
                        + cmd.Parameters[4].Value.ToString() + "; "
                        + cmd.Parameters[5].Value.ToString() + "; "
                        + cmd.Parameters[6].Value.ToString() + "; "
                        + cmd.Parameters[7].Value.ToString() + "; "
                        + cmd.Parameters[8].Value.ToString() + "; "
                        + cmd.Parameters[9].Value.ToString() + "; "
                        + cmd.Parameters[10].Value.ToString() + "; "
                        + cmd.Parameters[11].Value.ToString() + "; "
                    );
                    ioLogFile.WriteLine();


                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                catch (SqlException ex)
                {
                    if (conn.State != System.Data.ConnectionState.Closed) conn.Close();
                    ioLogFile.WriteLine(ex.Message);
                }
            }

            ioLogFile.Close();
            ioLogFile.Dispose();
        }
    }
}
