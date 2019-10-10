//LocationDAC.cs

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using YtoX.Entities;

namespace YtoX.Core.Storage
{
    public class LocationDAC
    {
        #region Methods

        public static List<Position> FetchAll()
        {
            List<Position> all = new List<Position>();
            using (DAC dac = new DAC())
            {
                using (DbDataReader reader = dac.FetchRecords("SELECT * FROM Location;"))
                {
                    while (reader.Read())
                    {
                        all.Add(Fetch(reader));
                    }
                }
            }
            return all;
        }

        public static void Update(Position pos)
        {
            using (DAC dac = new DAC())
            {
                string statement = string.Format("UPDATE Location SET Latitude = {0}, Longitude = {1}, FetchedOn = '{2}' WHERE Key = '{3}';", pos.Latitude, pos.Longitude, pos.FetchedOn, pos.Key);
                dac.ExecuteCommand(statement);
            }
        }

        #endregion

        #region Internal-Methods

        private static Position Fetch(DbDataReader reader)
        {
            Position pos = new Position();
            try
            {
                pos.Key = (string)reader["Key"];
                if (!Convert.IsDBNull(reader["Latitude"])) pos.Latitude = (double)reader["Latitude"];
                if (!Convert.IsDBNull(reader["Longitude"])) pos.Longitude = (double)reader["Longitude"];
                if (!Convert.IsDBNull(reader["FetchedOn"])) pos.FetchedOn = Convert.ToDateTime((string)reader["FetchedOn"]);

                if (double.IsInfinity(pos.Latitude)) pos.Latitude = double.MaxValue;
                if (double.IsInfinity(pos.Longitude)) pos.Longitude = double.MaxValue;
            }
            catch { }

            return pos;
        }

        #endregion
    }
}
