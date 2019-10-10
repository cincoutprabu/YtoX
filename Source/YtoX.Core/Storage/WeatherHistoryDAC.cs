//WeatherHistoryDAC.cs

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

using YtoX.Entities;

namespace YtoX.Core.Storage
{
    public class WeatherHistoryDAC
    {
        #region Methods

        public static List<WeatherEntry> FetchAll()
        {
            List<WeatherEntry> all = new List<WeatherEntry>();
            using (DAC dac = new DAC())
            {
                using (DbDataReader reader = dac.FetchRecords("SELECT * FROM WeatherHistory;"))
                {
                    while (reader.Read())
                    {
                        all.Add(Fetch(reader));
                    }
                }
            }
            return all;
        }

        public static void Clear()
        {
            using (DAC dac = new DAC())
            {
                string statement = string.Format("DELETE FROM WeatherHistory;");
                dac.ExecuteCommand(statement);
            }
        }

        public static void Insert(WeatherEntry entry)
        {
            using (DAC dac = new DAC())
            {
                string statement = string.Format("INSERT INTO WeatherHistory (Latitude, Longitude, Temperature, Forecast, FetchedOn) VALUES ({0}, {1}, {2}, '{3}', '{4}');", entry.Latitude, entry.Longitude, entry.Temperature, entry.Forecast, entry.FetchedOn);
                dac.ExecuteCommand(statement);
            }
        }

        #endregion

        #region Internal-Methods

        private static WeatherEntry Fetch(DbDataReader reader)
        {
            WeatherEntry entry = new WeatherEntry();
            try
            {
                if (!Convert.IsDBNull(reader["Latitude"])) entry.Latitude = (double)reader["Latitude"];
                if (!Convert.IsDBNull(reader["Longitude"])) entry.Longitude = (double)reader["Longitude"];
                if (!Convert.IsDBNull(reader["Temperature"])) entry.Temperature = (double)reader["Temperature"];
                if (!Convert.IsDBNull(reader["Forecast"])) entry.Forecast = (string)reader["Forecast"];
                if (!Convert.IsDBNull(reader["FetchedOn"])) entry.FetchedOn = Convert.ToDateTime((string)reader["FetchedOn"]);

                if (double.IsInfinity(entry.Latitude)) entry.Latitude = double.MaxValue;
                if (double.IsInfinity(entry.Longitude)) entry.Longitude = double.MaxValue;
                if (double.IsInfinity(entry.Temperature)) entry.Temperature = double.MaxValue;
            }
            catch { }

            return entry;
        }

        #endregion
    }
}
