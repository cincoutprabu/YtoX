//WeatherHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YtoX.Entities;
using YtoX.Core.Location;
using YtoX.Core.Storage;

namespace YtoX.Core.Weather
{
    public class WeatherHelper
    {
        public static bool WeatherRead = false;
        public static WeatherEntry CurrentWeather = null;

        #region Methods

        public async static Task ReadWeather()
        {
            WeatherRead = false;

            //ignore if weather is read very recently
            if (CurrentWeather != null && DateTime.Now.Subtract(CurrentWeather.FetchedOn).TotalMinutes < Constants.WEATHER_READ_INTERVAL)
            {
                return;
            }

            try
            {
                if (LocationHelper.CurrentPosition == null)
                {
                    await LocationHelper.ReadLocation(false);
                }

                if (LocationHelper.CurrentPosition != null)
                {
                    WeatherEntry entry = new WeatherEntry();
                    entry.Latitude = LocationHelper.CurrentPosition.Coordinate.Latitude;
                    entry.Longitude = LocationHelper.CurrentPosition.Coordinate.Longitude;

                    WeatherDetails weatherDetails = Wunderground.GetWeather(entry.Latitude, entry.Longitude);
                    if (weatherDetails != null)
                    {
                        entry.Temperature = Math.Round(weatherDetails.CurrentWeather.Temperature_C_Min, 2);
                    }

                    entry.Forecast = Wunderground.GetForecast(entry.Latitude, entry.Longitude);
                    entry.FetchedOn = DateTime.Now;
                    CurrentWeather = entry;
                    WeatherHistoryDAC.Insert(CurrentWeather);

                    WeatherRead = true;
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion
    }
}
