//WeatherInfo.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Core.Weather
{
    public class WeatherInfo
    {
        #region Properties

        public DateTime ObservedOn { get; set; }
        public DateTime WeatherDate { get; set; }
        public float Temperature_C_Min { get; set; }
        public float Temperature_C_Max { get; set; }
        public float Temperature_F_Min { get; set; }
        public float Temperature_F_Max { get; set; }
        public string Description { get; set; }
        public float WindSpeed_kmph { get; set; }
        public string WindDirection { get; set; }
        public string Humidity { get; set; }

        #endregion

        #region Constructors

        public WeatherInfo()
        {
            ObservedOn = default(DateTime);
            WeatherDate = default(DateTime);
            Temperature_C_Min = float.NaN;
            Temperature_C_Max = float.NaN;
            Temperature_F_Min = float.NaN;
            Temperature_F_Max = float.NaN;
            Description = string.Empty;
            WindSpeed_kmph = float.NaN;
            WindDirection = string.Empty;
            Humidity = string.Empty;
        }

        #endregion
    }

    public class WeatherDetails
    {
        #region Properties

        public WeatherInfo CurrentWeather { get; set; }
        public List<WeatherInfo> FutureWeathers { get; set; }

        #endregion

        #region Constructors

        public WeatherDetails()
        {
            CurrentWeather = new WeatherInfo();
            FutureWeathers = new List<WeatherInfo>();
        }

        #endregion
    }
}
