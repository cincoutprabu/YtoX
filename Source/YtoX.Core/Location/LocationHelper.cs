//LocationHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Devices.Geolocation;

using YtoX.Entities;
using YtoX.Core.Storage;

namespace YtoX.Core.Location
{
    public class LocationHelper
    {
        private static Geolocator locator = new Geolocator();

        public static Geoposition CurrentPosition = null;
        public static List<LocationEntry> History = new List<LocationEntry>();

        #region Methods

        public static void StartTracking()
        {
            try
            {
                locator.StatusChanged += locator_StatusChanged;
                locator.PositionChanged += locator_PositionChanged;

                Log.Write("Location Tracking Started");
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void StopTracking()
        {
            if (locator != null)
            {
                locator.StatusChanged -= locator_StatusChanged;
                locator.PositionChanged -= locator_PositionChanged;

                Log.Write("Location Tracking Stopped");
            }
        }

        public static void LoadHistory()
        {
            History.Clear();
            History.AddRange(LocationHistoryDAC.FetchAll());
        }

        public async static Task ReadLocation(bool saveInHistory)
        {
            try
            {
                CurrentPosition = await locator.GetGeopositionAsync().AsTask();
                if (saveInHistory) SaveCurrentLocation();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static Position FromGeoposition(Geoposition p)
        {
            return new Position()
            {
                Key = string.Empty,
                Latitude = p.Coordinate.Latitude,
                Longitude = p.Coordinate.Longitude,
                FetchedOn = DateTime.Now
            };
        }

        private static void SaveCurrentLocation()
        {
            Position pos = FromGeoposition(CurrentPosition);
            LocationEntry entry = KnownLocations.IsKnown(pos);
            History.Add(entry);
            LocationHistoryDAC.Insert(entry);
        }

        public static LocationEntry GetLastVisit(Position pos)
        {
            LocationEntry lastVisit = null;
            foreach (var entry in History)
            {
                double distance = Position.GetDistance(pos, entry);
                if (distance <= Constants.VISITED_DISTANCE_THRESHOLD)
                {
                    if (lastVisit == null) lastVisit = entry;
                    else
                    {
                        if (lastVisit.VisitedOn < entry.VisitedOn)
                        {
                            lastVisit = entry;
                        }
                    }
                }
            }

            return lastVisit;
        }

        #endregion

        #region Events

        static void locator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            switch (args.Status)
            {
                case PositionStatus.Ready:
                    break;
                case PositionStatus.Initializing:
                    break;
                case PositionStatus.NoData:
                    break;
                case PositionStatus.Disabled:
                    break;
                case PositionStatus.NotInitialized:
                    break;
                case PositionStatus.NotAvailable:
                    break;
                default:
                    break;
            }
        }

        static void locator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            try
            {
                //ignore duplicate position
                if (CurrentPosition != null)
                {
                    if (CurrentPosition.Coordinate.Latitude == args.Position.Coordinate.Latitude &&
                        CurrentPosition.Coordinate.Longitude == args.Position.Coordinate.Longitude)
                    {
                        return;
                    }
                }

                CurrentPosition = args.Position;
                SaveCurrentLocation();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion
    }
}
