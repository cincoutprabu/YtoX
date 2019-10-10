//YahooWeather.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YtoX.Core.Helpers;

namespace YtoX.Core.Weather
{
    public class YahooWeather
    {
        #region Methods

        public static void GetWeather()
        {
            try
            {
                int woeid = 2459115;
                string unit = "c"; //'c' or 'f'
                string url = string.Format("http://weather.yahooapis.com/forecastrss?w={0}&u={1}", woeid, unit);

                string xml = WebHelper.ReadUrl(url);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion
    }
}
