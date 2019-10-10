//WeatherEntry.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Entities
{
    public class WeatherEntry
    {
        #region Properties

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Temperature { get; set; }
        public string Forecast { get; set; }
        public DateTime FetchedOn { get; set; }

        #endregion

        #region Methods

        public WeatherEntry()
        {
            this.Latitude = double.MaxValue;
            this.Longitude = double.MaxValue;
            this.Temperature = double.MaxValue;
            this.Forecast = string.Empty;
            this.FetchedOn = default(DateTime);
        }

        #endregion
    }
}
