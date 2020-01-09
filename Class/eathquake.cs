using System;

namespace eathquakeJSON
{
    public interface IEathquake
    {
        string id { get; set; }           //Data Type String. Typical Values A (generally) two-character network identifier with a (generally) eight-character network-assigned code. Description A unique identifier for the event. This is the current preferred id for the event, and may change over time. See the "ids" GeoJSON format property.
        decimal mag { get; set; }         //Data Type Decimal. Typical Values [-1.0, 10.0]. Description The magnitude for the event.
        string place { get; set; }        //Data Type String. Description Textual description of named geographic region near to the event. This may be a city name, or a Flinn-Engdahl Region name.
        long time { get; set; }           //Data Type Long Integer. Description Time when the event occurred. Times are reported in milliseconds since the epoch ( 1970-01-01T00:00:00.000Z), and do not include leap seconds. In certain output formats, the date is formatted for readability.
        long updated { get; set; }        //Data Type Long Integer. Description Time when the event was most recently updated. Times are reported in milliseconds since the epoch. In certain output formats, the date is formatted for readability.
        int tz { get; set; }              //Data Type Integer. Typical Values [-1200, +1200]. Description Timezone offset from UTC in minutes at the event epicenter.
        string url { get; set; }          //Data Type String. Description Link to USGS Event Page for event.
        string detailUrl { get; set; }    //Data Type String. Description Link to GeoJSON detail feed from a GeoJSON summary feed. NOTE: When searching and using geojson with callback, no callback is included in the detail url.
        int tsunami { get; set; }         //Data Type Integer. Description This flag is set to "1" for large events in oceanic regions and "0" otherwise. The existence or value of this flag does not indicate if a tsunami actually did or will exist. If the flag value is "1", the event will include a link to the NOAA Tsunami website for tsunami information. The USGS is not responsible for Tsunami warning; we are simply providing a link to the authoritative NOAA source.
        int felt { get; set; }            //Data Type Integer. Typical Values [44, 843]. Description The total number of felt reports submitted to the DYFI? system.
        decimal cdi { get; set; }         //Data Type Decimal. Typical Values [0.0, 10.0]. Description The maximum reported intensity for the event. Computed by DYFI. While typically reported as a roman numeral, for the purposes of this API, intensity is expected as the decimal equivalent of the roman numeral. Learn more about magnitude vs. intensity.
        decimal mmi { get; set; }         //Data Type Decimal. Typical Values [0.0, 10.0]. Description The maximum estimated instrumental intensity for the event. Computed by ShakeMap. While typically reported as a roman numeral, for the purposes of this API, intensity is expected as the decimal equivalent of the roman numeral. Learn more about magnitude vs. intensity.
        string alert { get; set; }        //Data Type String. Typical Values “green”, “yellow”, “orange”, “red”. Description The alert level from the PAGER earthquake impact scale.
        string status { get; set; }       //Data Type String. Typical Values “automatic”, “reviewed”, “deleted”. Description Indicates whether the event has been reviewed by a human. Additional Information Status is either automatic or reviewed. Automatic events are directly posted by automatic processing systems and have not been verified or altered by a human. Reviewed events have been looked at by a human. The level of review can range from a quick validity check to a careful reanalysis of the event.
        int sig { get; set; }             //Data Type Integer. Typical Values [0, 1000]. Description A number describing how significant the event is. Larger numbers indicate a more significant event. This value is determined on a number of factors, including: magnitude, maximum MMI, felt reports, and estimated impact.
        string net { get; set; }          //Data Type String. Typical Values ak, at, ci, hv, ld, mb, nc, nm, nn, pr, pt, se, us, uu, uw. Description The ID of a data contributor. Identifies the network considered to be the preferred source of information for this event.
        string code { get; set; }         //Data Type String. Typical Values "2013lgaz", "c000f1jy", "71935551". Description An identifying code assigned by - and unique from - the corresponding source for the event.
        string ids { get; set; }          //Data Type String. Typical Values ",ci15296281,us2013mqbd,at00mji9pf,". Description A comma-separated list of event ids that are associated to an event.
        string sources { get; set; }      //Data Type String. Typical Values ",us,nc,ci,". Description A comma-separated list of network contributors.
        string types { get; set; }        //Data Type String. Typical Values “,cap,dyfi,general-link,origin,p-wave-travel-times,phase-data,”. Description A comma-separated list of product types associated to this event.
        int nst { get; set; }             //Data Type Integer. Description The total number of seismic stations used to determine earthquake location. Additional Information Number of seismic stations which reported P- and S-arrival times for this earthquake. This number may be larger than the Number of Phases Used if arrival times are rejected because the distance to a seismic station exceeds the maximum allowable distance or because the arrival-time observation is inconsistent with the solution.
        decimal dmin { get; set; }        //Data Type Decimal. Typical Values [0.4, 7.1]. Description Horizontal distance from the epicenter to the nearest station (in degrees). 1 degree is approximately 111.2 kilometers. In general, the smaller this number, the more reliable is the calculated depth of the earthquake.
        decimal rms { get; set; }         //Data Type Decimal. Typical Values [0.13,1.39]. Description The root-mean-square (RMS) travel time residual, in sec, using all weights. This parameter provides a measure of the fit of the observed arrival times to the predicted arrival times for this location. Smaller numbers reflect a better fit of the data. The value is dependent on the accuracy of the velocity model used to compute the earthquake location, the quality weights assigned to the arrival time data, and the procedure used to locate the earthquake.
        decimal gap { get; set; }         //Data Type Decimal. Typical Values [0.0, 180.0]. Description The largest azimuthal gap between azimuthally adjacent stations (in degrees). In general, the smaller this number, the more reliable is the calculated horizontal position of the earthquake. Earthquake locations in which the azimuthal gap exceeds 180 degrees typically have large location and depth uncertainties.
        string magType { get; set; }      //Data Type String. Typical Values “Md”, “Ml”, “Ms”, “Mw”, “Me”, “Mi”, “Mb”, “MLg”. Description The method or algorithm used to calculate the preferred magnitude for the event.
        string type { get; set; }         //Data Type String. Typical Values “earthquake”, “quarry”. Description Type of seismic event.
        string title { get; set; }        //Data Type String. Title of seismic event.

        IEathquakeGeometry eathquakeGeometry { get; set; }

        //Magnitude Types table link
        //https://www.usgs.gov/natural-hazards/earthquake-hazards/science/magnitude-types?qt-science_center_objects=0#qt-science_center_objects
    }

    class Eathquake : IEathquake
    {
        
        IEathquakeGeometry coordinates;

        public Eathquake()
        {
            coordinates = new EathquakeGeometry();            
        }

        public Eathquake(string _id,
            decimal _mag, string _place, long _time, long _updated, string _url, string _detailUrl, int _tsunami, int _sig,
            string _net, string _code, string _ids, string _sources, string _types, decimal _rms, string _magType, string _type, string _title)
        {
            id = _id;
            mag = _mag;
            place = _place;
            time = _time;
            updated = _updated;
            url = _url;
            detailUrl = _detailUrl;
            tsunami = _tsunami;
            sig = _sig;
            net = _net;
            code = _code;
            ids = _ids;
            sources = _sources;
            types = _types;
            rms = _rms;
            magType = _magType;
            type = _type;
            title = _title;
        }

        #region properties   

        public string id { get; set; }
        public decimal mag { get; set; }
        public string place { get; set; }
        public long time { get; set; }
        public long updated { get; set; }
        public int tz { get; set; }
        public string url { get; set; }
        public string detailUrl { get; set; }
        public int tsunami { get; set; }
        public int felt { get; set; }
        public decimal cdi { get; set; }
        public decimal mmi { get; set; }
        public string alert { get; set; }
        public string status { get; set; }
        public int sig { get; set; }
        public string net { get; set; }
        public string code { get; set; }
        public string ids { get; set; }
        public string sources { get; set; }
        public string types { get; set; }
        public int nst { get; set; }
        public decimal dmin { get; set; }
        public decimal gap { get; set; }
        public decimal rms { get; set; }
        public string magType { get; set; }
        public string type { get; set; }
        public string title { get; set; }

        public IEathquakeGeometry eathquakeGeometry
        {
            get
            {
                return coordinates;
            }
            set
            {
                coordinates = value;
            }
        }

        #endregion
    }
}

