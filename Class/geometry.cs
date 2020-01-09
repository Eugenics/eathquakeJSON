using System;

namespace eathquakeJSON
{
    public interface IEathquakeGeometry
    {
        decimal longitude {get; set;}   //Data Type Decimal. Typical Values [-180.0, 180.0]. Description Decimal degrees longitude. Negative values for western longitudes.             
        decimal latitude {get; set;}    //Data Type Decimal. Typical Values [-90.0, 90.0]. Description Decimal degrees latitude. Negative values for southern latitudes.
        decimal depth {get; set;}       //Data Type Decimal. Typical Values [0, 1000]. Description Depth of the event in kilometers.        
    }

    class EathquakeGeometry:IEathquakeGeometry
    {
        public EathquakeGeometry()
        {
            type = "Point";
        }

    #region properties
    
        public decimal longitude {get; set;}
        public decimal latitude {get; set;}
        public decimal depth {get; set;}
        public string type {get; set;}
    
    #endregion
    }

}