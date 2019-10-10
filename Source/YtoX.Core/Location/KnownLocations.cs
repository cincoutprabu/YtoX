//KnownLocations.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YtoX.Entities;
using YtoX.Core.Storage;

namespace YtoX.Core.Location
{
    public class KnownLocations
    {
        public static string HOME = "HOME";
        public static string WORK = "WORK";
        public static string GYM = "GYM";
        public static string BIKE_SHOWROOM = "BIKE_SHOWROOM";
        public static string CAR_SHOWROOM = "CAR_SHOWROOM";

        private static List<Position> List = new List<Position>();

        #region Methods

        public static void Load()
        {
            List.Clear();
            List.AddRange(LocationDAC.FetchAll());
        }

        public static void Save()
        {
            foreach (var p in List)
            {
                LocationDAC.Update(p);
            }
        }

        public static Position Get(string key)
        {
            return List.FirstOrDefault(obj => obj.Key == key);
        }

        public static void Set(Position pos)
        {
            foreach (var p in List)
            {
                if (p.Key == pos.Key)
                {
                    p.Latitude = pos.Latitude;
                    p.Longitude = pos.Longitude;
                    p.FetchedOn = pos.FetchedOn;
                    break;
                }
            }
        }

        public static LocationEntry IsKnown(Position pos)
        {
            LocationEntry entry = new LocationEntry()
            {
                Latitude = pos.Latitude,
                Longitude = pos.Longitude,
                VisitedOn = pos.FetchedOn
            };

            foreach (var p in List)
            {
                if (Position.GetDistance(p, pos) <= Constants.VISITED_DISTANCE_THRESHOLD)
                {
                    entry.IsKnown = true;
                    entry.KnownKey = p.Key;
                }
            }

            return entry;
        }

        #endregion
    }
}
