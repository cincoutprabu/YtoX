//Wunderground.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Devices.Geolocation;

using YtoX.Core.Helpers;
using YtoX.Core.Location;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;

using YtoX.Entities;

namespace YtoX.Core.Weather
{
    public class Wunderground
    {
        private static string ApiKey = "728d0af71e70e805";

        #region Methods

        public static Position GeoLookup(double latitude, double longitude)
        {
            try
            {
                string url = string.Format("http://api.wunderground.com/api/{0}/geolookup/q/{1},{2}.json", ApiKey, latitude, longitude);

                string jsonString = WebHelper.ReadUrl(url);
                BsonDocument json = BsonDocument.Parse(jsonString);
                BsonDocument location = (BsonDocument)json["location"];

                //build 'Position' object
                Position position = new Position();
                position.Latitude = latitude;
                position.Longitude = longitude;
                position.FetchedOn = DateTime.Now;
                return position;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return null;
        }

        public static string GetForecast(double latitude, double longitude)
        {
            try
            {
                string url = string.Format("http://api.wunderground.com/api/{0}/forecast/q/{1},{2}.json", ApiKey, latitude, longitude);

                string jsonString = WebHelper.ReadUrl(url);
                BsonDocument json = BsonDocument.Parse(jsonString);

                //get forecast string
                BsonArray forecastArray = (BsonArray)(((BsonDocument)(((BsonDocument)json["forecast"])["txt_forecast"]))["forecastday"]);
                if (forecastArray.Count > 0)
                {
                    return ((BsonDocument)forecastArray[0])["fcttext"].ToString();
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return string.Empty;
        }

        public static WeatherDetails GetWeather(double latitude, double longitude)
        {
            try
            {
                string url = string.Format("http://api.wunderground.com/api/{0}/conditions/q/{1},{2}.json", ApiKey, latitude, longitude);

                string jsonString = WebHelper.ReadUrl(url);
                BsonDocument json = BsonDocument.Parse(jsonString);
                BsonDocument observation = (BsonDocument)json["current_observation"];

                //build 'WeatherDetails' object
                WeatherDetails weatherDetails = new WeatherDetails();
                weatherDetails.CurrentWeather.ObservedOn = DateTime.Now;
                weatherDetails.CurrentWeather.WeatherDate = DateTime.Now.Date;
                weatherDetails.CurrentWeather.Temperature_C_Min = float.Parse(observation["temp_c"].ToString());
                weatherDetails.CurrentWeather.Temperature_C_Max = weatherDetails.CurrentWeather.Temperature_C_Min;
                weatherDetails.CurrentWeather.Temperature_F_Min = float.Parse(observation["temp_f"].ToString());
                weatherDetails.CurrentWeather.Temperature_F_Max = weatherDetails.CurrentWeather.Temperature_F_Min;
                weatherDetails.CurrentWeather.Description = observation["weather"].ToString();
                weatherDetails.CurrentWeather.WindSpeed_kmph = float.Parse(observation["wind_kph"].ToString());
                weatherDetails.CurrentWeather.WindDirection = observation["wind_dir"].ToString();
                weatherDetails.CurrentWeather.Humidity = observation["relative_humidity"].ToString();
                return weatherDetails;
                //return weatherDetails.CurrentWeather.Temperature_C_Min;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return null;
            //return -1;
        }

        #endregion
    }
}
