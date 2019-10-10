//LocationHistoryDAC.cs

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;

using YtoX.Entities;

namespace YtoX.Core.Storage
{
    public class LocationHistoryDAC
    {
        #region Methods

        public static List<LocationEntry> FetchAll()
        {
            List<LocationEntry> all = new List<LocationEntry>();
            using (DAC dac = new DAC())
            {
                using (DbDataReader reader = dac.FetchRecords("SELECT * FROM LocationHistory;"))
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
                string statement = string.Format("DELETE FROM LocationHistory;");
                dac.ExecuteCommand(statement);
            }
        }

        public static void Insert(LocationEntry entry)
        {
            using (DAC dac = new DAC())
            {
                string statement = string.Format("INSERT INTO LocationHistory (Latitude, Longitude, VisitedOn, IsKnown, KnownKey) VALUES ({0}, {1}, '{2}', {3}, '{4}');", entry.Latitude, entry.Longitude, entry.VisitedOn, entry.IsKnown ? 1 : 0, entry.KnownKey);
                dac.ExecuteCommand(statement);
            }
        }

        #endregion

        #region Internal-Methods

        private static LocationEntry Fetch(DbDataReader reader)
        {
            LocationEntry entry = new LocationEntry();
            try
            {
                if (!Convert.IsDBNull(reader["Latitude"])) entry.Latitude = (double)reader["Latitude"];
                if (!Convert.IsDBNull(reader["Longitude"])) entry.Longitude = (double)reader["Longitude"];
                if (!Convert.IsDBNull(reader["VisitedOn"])) entry.VisitedOn = Convert.ToDateTime((string)reader["VisitedOn"]);
                entry.IsKnown = (bool)reader["IsKnown"];
                entry.KnownKey = (string)reader["KnownKey"];

                if (double.IsInfinity(entry.Latitude)) entry.Latitude = double.MaxValue;
                if (double.IsInfinity(entry.Longitude)) entry.Longitude = double.MaxValue;
            }
            catch { }

            return entry;
        }

        #endregion
    }
}
