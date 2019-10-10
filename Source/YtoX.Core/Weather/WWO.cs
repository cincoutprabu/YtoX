//WWO.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using YtoX.Core.Helpers;

namespace YtoX.Core.Weather
{
    /// <summary>
    /// To get weather information from WorldWeatherOnline.com
    /// </summary>
    public class WWO
    {
        #region Methods

        public static WeatherDetails GetWeather(double latitude, double longitude)
        {
            try
            {
                string apiKey = "c9f0be46ae133902122810";
                string url = string.Format("http://free.worldweatheronline.com/feed/weather.ashx?q={0},{1}&format=xml&num_of_days=1&key={2}", latitude, longitude, apiKey);

                string xml = WebHelper.ReadUrl(url);
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);

                WeatherDetails weatherDetails = new WeatherDetails();

                //read current weather
                XmlNode currentNode = document.SelectSingleNode("data/current_condition");
                if (currentNode != null)
                {
                    WeatherInfo weather = new WeatherInfo();
                    weather.ObservedOn = DateTime.Now;
                    weather.WeatherDate = DateTime.Now.Date;
                    weather.Temperature_C_Min = Convert.ToSingle(currentNode.SelectSingleNode("temp_C").InnerText);
                    weather.Temperature_C_Max = Convert.ToSingle(currentNode.SelectSingleNode("temp_C").InnerText);
                    weather.Temperature_F_Min = Convert.ToSingle(currentNode.SelectSingleNode("temp_F").InnerText);
                    weather.Temperature_F_Max = Convert.ToSingle(currentNode.SelectSingleNode("temp_F").InnerText);
                    weather.Description = currentNode.SelectSingleNode("weatherDesc").InnerText;
                    weather.WindSpeed_kmph = Convert.ToSingle(currentNode.SelectSingleNode("windspeedKmph").InnerText);
                    weather.WindDirection = currentNode.SelectSingleNode("winddir16Point").InnerText;
                    weather.Humidity = currentNode.SelectSingleNode("humidity").InnerText;

                    weatherDetails.CurrentWeather = weather;
                }

                //read future weathers
                XmlNodeList futureNodes = document.SelectNodes("data/weather");
                foreach (XmlNode node in futureNodes)
                {
                    WeatherInfo weather = new WeatherInfo();
                    weather.ObservedOn = DateTime.Now;
                    weather.WeatherDate = DateTime.Now.AddDays(1).Date;
                    weather.Temperature_C_Min = Convert.ToSingle(node.SelectSingleNode("tempMinC").InnerText);
                    weather.Temperature_C_Max = Convert.ToSingle(node.SelectSingleNode("tempMaxC").InnerText);
                    weather.Temperature_F_Min = Convert.ToSingle(node.SelectSingleNode("tempMinF").InnerText);
                    weather.Temperature_F_Max = Convert.ToSingle(node.SelectSingleNode("tempMaxF").InnerText);
                    weather.Description = node.SelectSingleNode("weatherDesc").InnerText;
                    weather.WindSpeed_kmph = Convert.ToSingle(node.SelectSingleNode("windspeedKmph").InnerText);
                    weather.WindDirection = node.SelectSingleNode("winddirection").InnerText;
                    weather.Humidity = string.Empty;

                    weatherDetails.FutureWeathers.Add(weather);
                }

                return weatherDetails;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return null;
        }

        #endregion
    }
}
