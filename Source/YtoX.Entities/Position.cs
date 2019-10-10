//Position.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Entities
{
    public class Position
    {
        #region Properties

        public string Key { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime FetchedOn { get; set; }

        #endregion

        #region Constructors

        public Position()
        {
            this.Key = string.Empty;
            this.Latitude = double.MaxValue;
            this.Longitude = double.MaxValue;
            this.FetchedOn = default(DateTime);
        }

        #endregion

        #region Methods

        public bool IsValid()
        {
            //return !double.IsNaN(this.Latitude) && !double.IsNaN(this.Longitude);
            return this.Latitude != double.MaxValue && this.Longitude != double.MaxValue;
        }

        #endregion

        #region Helper-Methods

        public static double GetDistance(Position p1, Position p2)
        {
            //The Haversine formula according to Dr. Math.
            //http://mathforum.org/library/drmath/view/51879.html

            double lat1InRad = p1.Latitude * (Math.PI / 180.0);
            double long1InRad = p1.Longitude * (Math.PI / 180.0);
            double lat2InRad = p2.Latitude * (Math.PI / 180.0);
            double long2InRad = p2.Longitude * (Math.PI / 180.0);

            double longitude = long2InRad - long1InRad;
            double latitude = lat2InRad - lat1InRad;

            //Intermediate result a
            double a = Math.Pow(Math.Sin(latitude / 2.0), 2.0) +
                       Math.Cos(lat1InRad) * Math.Cos(lat2InRad) *
                       Math.Pow(Math.Sin(longitude / 2.0), 2.0);

            //Intermediate result c (great circle distance in Radians)
            double c = 2.0 * Math.Asin(Math.Sqrt(a));

            //const Double kEarthRadiusMiles = 3956.0;
            const double kEarthRadiusKms = 6376.5;
            double distance = kEarthRadiusKms * c;
            return distance;
        }

        public static double GetDistance(Position p1, LocationEntry p2)
        {
            return GetDistance(p1, new Position() { Latitude = p2.Latitude, Longitude = p2.Longitude });
        }

        #endregion
    }
}
