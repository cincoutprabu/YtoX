//Core.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAPICodePack.ApplicationServices;

using YtoX.Entities;
using YtoX.Core;
using YtoX.Core.Activities;
using YtoX.Core.Helpers;
using YtoX.Core.Location;
using YtoX.Core.News;
using YtoX.Core.Weather;

namespace YtoX.Core
{
    public class Execution
    {
        private static Dictionary<string, DateTime> RecipeNotificationHistory = new Dictionary<string, DateTime>();

        #region Methods

        public async static Task Run()
        {
            //ExecuteAllRecipes();
            ExecuteNewsRecipes();
            await ExecuteWeatherRecipes();
            ExecuteLocationRecipes();
            ExecuteLifestyleRecipes();
        }

        private static void ExecuteAllRecipes()
        {
            try
            {
                foreach (var group in Repository.Instance.Groups)
                {
                    foreach (var recipe in group.Recipes)
                    {
                        if (recipe.IsAvailable && recipe.IsEnabled)
                        {
                            //if (recipe.Trigger.Condition())
                            //{
                            //    recipe.Activity.Execute();
                            //}
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void ExecuteNewsRecipes()
        {
            try
            {
                if (NewsHelper.ReadNews())
                {
                    var followTopicsRecipe = Repository.Instance.GetRecipe("Follow Topics");
                    if (followTopicsRecipe != null && followTopicsRecipe.IsAvailable && followTopicsRecipe.IsEnabled)
                    {
                        NewsHelper.GetMatchingAndSendEmail(Constants.NEWS_TOPICS.Split(','));
                    }

                    var followPeopleRecipe = Repository.Instance.GetRecipe("Follow People");
                    if (followPeopleRecipe != null && followPeopleRecipe.IsAvailable && followPeopleRecipe.IsEnabled)
                    {
                        NewsHelper.GetMatchingAndSendEmail(Constants.NEWS_PEOPLE.Split(','));
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public async static Task ExecuteWeatherRecipes()
        {
            try
            {
                await WeatherHelper.ReadWeather();
                if (!WeatherHelper.WeatherRead) return;

                var dropRecipe = Repository.Instance.GetRecipe("Temperature Drop");
                if (dropRecipe != null && dropRecipe.IsAvailable && dropRecipe.IsEnabled)
                {
                    if (WeatherHelper.CurrentWeather != null && WeatherHelper.CurrentWeather.Temperature <= Constants.TEMPERATURE_ALERT_THRESHOLD)
                    {
                        var arguments = new Dictionary<string, object>()
                        { 
                            { "HeaderText", "Weather Alert!" },
                            { "AlertText", "Temperature has dropped below " + Constants.TEMPERATURE_ALERT_THRESHOLD + " degree Celsius." },
                            { "Persistent", false },
                            { "Delay", 500 }
                        };
                        SendNotification(dropRecipe, arguments);
                    }
                }

                var rainRecipe = Repository.Instance.GetRecipe("Rain");
                if (rainRecipe != null && rainRecipe.IsAvailable && rainRecipe.IsEnabled)
                {
                    if (WeatherHelper.CurrentWeather != null && WeatherHelper.CurrentWeather.Forecast.ToLower().Contains("rain"))
                    {
                        var arguments = new Dictionary<string, object>()
                        {
                            { "HeaderText", "Love soaking in rain?" },
                            { "AlertText", "If NO, pack your umbrella now. There is a rain forecast." },
                            { "Persistent", true },
                            { "Delay", 500 }
                        };
                        SendNotification(rainRecipe, arguments);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void ExecuteLocationRecipes()
        {
            try
            {
                //execute buy-milk recipe
                var buyMilkRecipe = Repository.Instance.GetRecipe("Buy Milk");
                if (buyMilkRecipe != null && buyMilkRecipe.IsAvailable && buyMilkRecipe.IsEnabled)
                {
                    var work = KnownLocations.Get(KnownLocations.WORK);
                    if (VisitedThisPlaceWithinLastNMinutes(work, Constants.LEFT_FROM_DURATION_THRESHOLD))
                    {
                        var arguments = new Dictionary<string, object>()
                        {
                            { "HeaderText", "Isn't your wife waiting?" },
                            { "AlertText", "Hope you have bought milk before going home." },
                            { "Persistent", true },
                            { "Delay", 500 }
                        };

                        buyMilkRecipe.ScopeHours = TimeSpan.FromMinutes(Constants.LEFT_FROM_DURATION_THRESHOLD).TotalHours + 1;
                        SendNotification(buyMilkRecipe, arguments);
                    }
                }

                //execute car-service recipe
                var carServiceRecipe = Repository.Instance.GetRecipe("Car Service");
                if (carServiceRecipe != null && carServiceRecipe.IsAvailable && carServiceRecipe.IsEnabled)
                {
                    var carShowroom = KnownLocations.Get(KnownLocations.CAR_SHOWROOM);
                    if (carShowroom.IsValid())
                    {
                        LocationEntry lastVisit = LocationHelper.GetLastVisit(carShowroom);
                        if (lastVisit != null)
                        {
                            var daysSinceVisited = DateTime.Now.Subtract(lastVisit.VisitedOn).TotalDays;
                            if (daysSinceVisited >= Constants.CAR_SERVICE_INTERVAL)
                            {
                                var arguments = new Dictionary<string, object>()
                                {
                                    { "HeaderText", "Your car is doing good?" },
                                    { "AlertText", "It's " + daysSinceVisited + " days since you visited the Car Showroom." },
                                    { "Persistent", true },
                                    { "Delay", 500 }
                                };
                                SendNotification(carServiceRecipe, arguments);
                            }
                        }
                    }
                }

                //execute gym recipe
                var gymRecipe = Repository.Instance.GetRecipe("Gym");
                if (gymRecipe != null && gymRecipe.IsAvailable && gymRecipe.IsEnabled)
                {
                    var gym = KnownLocations.Get(KnownLocations.GYM);
                    if (gym.IsValid())
                    {
                        LocationEntry lastVisit = LocationHelper.GetLastVisit(gym);
                        if (lastVisit != null)
                        {
                            var daysSinceVisited = DateTime.Now.Subtract(lastVisit.VisitedOn).TotalDays;
                            if (daysSinceVisited >= Constants.MAX_GYM_VACATION)
                            {
                                var arguments = new Dictionary<string, object>()
                                {
                                    { "HeaderText", "Aren't you health conscious?" },
                                    { "AlertText", "It's " + daysSinceVisited + " days since you visited the Gym." },
                                    { "Persistent", true },
                                    { "Delay", 500 }
                                };
                                SendNotification(gymRecipe, arguments);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        private static bool VisitedThisPlaceWithinLastNMinutes(Position pos, double minutesThreshold)
        {
            if (pos.IsValid())
            {
                LocationEntry lastVisit = LocationHelper.GetLastVisit(pos);
                if (lastVisit != null)
                {
                    var minutesSinceVisited = DateTime.Now.Subtract(lastVisit.VisitedOn).TotalMinutes;
                    return minutesSinceVisited <= minutesThreshold;
                }
            }

            return false;
        }

        public static void ExecuteLifestyleRecipes()
        {
            try
            {
                var meetingRecipe = Repository.Instance.GetRecipe("Meeting");
                if (meetingRecipe != null && meetingRecipe.IsAvailable && meetingRecipe.IsEnabled)
                {
                    if (BatteryHelper.SystemInUse) //if PC is being used
                    {
                        string firstAppointmentInfo = OutlookHelper.GetFirstAppointmentInfo();
                        if (firstAppointmentInfo != null)
                        {
                            int alertHour = Constants.TimesOfDay[Constants.MEETING_ALERT_HOUR];
                            if (DateTime.Now.Hour >= alertHour & DateTime.Now.Hour <= alertHour + 1)
                            {
                                var arguments = new Dictionary<string, object>()
                                {
                                    { "HeaderText", "You have a meeting!" },
                                    { "AlertText", firstAppointmentInfo },
                                    { "Persistent", true },
                                    { "Delay", 500 }
                                };
                                SendNotification(meetingRecipe, arguments);
                            }
                        }
                    }
                }

                var timeSheetRecipe = Repository.Instance.GetRecipe("TimeSheet");
                if (timeSheetRecipe != null && timeSheetRecipe.IsAvailable && timeSheetRecipe.IsEnabled)
                {
                    if (DateTime.Now.DayOfWeek != DayOfWeek.Saturday && DateTime.Now.DayOfWeek != DayOfWeek.Sunday)
                    {
                        var work = KnownLocations.Get(KnownLocations.WORK);
                        if (VisitedThisPlaceWithinLastNMinutes(work, 8 * 60)) //within last 8 hours
                        {
                            int alertHour = Constants.TimesOfDay[Constants.TIMESHEET_ALERT_HOUR];
                            if (DateTime.Now.Hour >= alertHour & DateTime.Now.Hour <= alertHour + 1)
                            {
                                var arguments = new Dictionary<string, object>()
                                {
                                    { "HeaderText", "Remember your day?" },
                                    { "AlertText", "Its time to fill your TimeSheet, before you leave for the day." },
                                    { "Persistent", true },
                                    { "Delay", 500 }
                                };
                                SendNotification(timeSheetRecipe, arguments);
                            }
                        }
                    }
                }

                var weeklyReportRecipe = Repository.Instance.GetRecipe("Weekly Report");
                if (weeklyReportRecipe != null && weeklyReportRecipe.IsAvailable && weeklyReportRecipe.IsEnabled)
                {
                    DayOfWeek dow = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), Constants.WEEKLY_SUMMARY_ALERT_DAY);
                    if (DateTime.Now.DayOfWeek == dow)
                    {
                        var work = KnownLocations.Get(KnownLocations.WORK);
                        if (VisitedThisPlaceWithinLastNMinutes(work, 8 * 60)) //within last 8 hours
                        {
                            int alertHour = Constants.TimesOfDay[Constants.WEEKLY_SUMMARY_ALERT_HOUR];
                            if (DateTime.Now.Hour >= alertHour & DateTime.Now.Hour <= alertHour + 1)
                            {
                                var arguments = new Dictionary<string, object>()
                                {
                                    { "HeaderText", "1 more task before weekend!" },
                                    { "AlertText", "Have you sent Weekly Summary Report to your manager?" },
                                    { "Persistent", true },
                                    { "Delay", 500 }
                                };
                                SendNotification(weeklyReportRecipe, arguments);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void ExecuteBusyRecipe(string chatClientName, string to, Action<string> action)
        {
            var busyRecipe = Repository.Instance.GetRecipe("Busy");
            if (busyRecipe != null && busyRecipe.IsAvailable && busyRecipe.IsEnabled)
            {
                if (OutlookHelper.InProgressMeetings > 0) //if a meeting is in progress
                {
                    action.Invoke(Constants.MEETING_AUTOREPLY_MESSAGE);
                    Log.Write(chatClientName + ": AutoReply sent to '" + to + "': '" + Constants.MEETING_AUTOREPLY_MESSAGE + "'");
                }
            }
        }

        public static void ExecuteBatteryLowRecipe(string chatClientName, string to, Action<string> action)
        {
            var batteryLowRecipe = Repository.Instance.GetRecipe("Battery Low");
            if (batteryLowRecipe != null && batteryLowRecipe.IsAvailable && batteryLowRecipe.IsEnabled)
            {
                if (BatteryHelper.BatteryLifePercent <= Constants.BATTERY_WARNING_THRESHOLD)
                {
                    action.Invoke(Constants.BATTERYLOW_AUTOREPLY_MESSAGE);
                    Log.Write(chatClientName + ": AutoReply sent to '" + to + "': '" + Constants.BATTERYLOW_AUTOREPLY_MESSAGE + "'");
                }
            }
        }

        public static void ExecuteMuteRecipe(bool doMute)
        {
            var muteRecipe = Repository.Instance.GetRecipe("Mute");
            if (muteRecipe != null && muteRecipe.IsAvailable && muteRecipe.IsEnabled)
            {
                if (doMute && !SystemHelper.IsMute())
                {
                    SystemHelper.Mute();
                }

                if (!doMute && SystemHelper.IsMute())
                {
                    SystemHelper.Mute();
                }
            }
        }

        private bool isRecording = false;
        public void ExecuteRecordRecipe(bool doRecord)
        {
            var recordRecipe = Repository.Instance.GetRecipe("Record");
            if (recordRecipe != null && recordRecipe.IsAvailable && recordRecipe.IsEnabled)
            {
                if (doRecord && !isRecording)
                {
                    AudioRecordActivity.Instance.Start();
                    isRecording = true;
                }

                if (!doRecord && isRecording)
                {
                    AudioRecordActivity.Instance.Stop();
                    isRecording = false;
                }
            }
        }

        public static void ExecuteDocumentsRecipe(string from, string to, bool overWrite)
        {
            try
            {
                var documentsRecipe = Repository.Instance.GetRecipe("Documents");
                if (documentsRecipe != null && documentsRecipe.IsAvailable && documentsRecipe.IsEnabled)
                {
                    File.Copy(from, to, overWrite);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void ExecuteFavoriteAppRecipe()
        {
            var shakeRecipe = Repository.Instance.GetRecipe("Favorite App");
            if (shakeRecipe != null && shakeRecipe.IsAvailable && shakeRecipe.IsEnabled)
            {
                LaunchHelper.LaunchApp(Constants.APP_TO_LAUNCH_ON_SHAKE);
            }
        }

        public static void ExecuteLandscapeRecipe()
        {
            var landscapeRecipe = Repository.Instance.GetRecipe("Landscape");
            if (landscapeRecipe != null && landscapeRecipe.IsAvailable && landscapeRecipe.IsEnabled)
            {
                LaunchHelper.LaunchApp(Constants.APP_TO_LAUNCH_ON_LANDSCAPE);
            }
        }

        #endregion

        #region Internal-Methods

        private static void SendNotification(Recipe recipe, Dictionary<string, object> notificationArgs)
        {
            if (RecipeNotificationHistory.ContainsKey(recipe.Name))
            {
                if (recipe.ScopeHours == -1) //-1 signifies that recipe shud be executed only once
                {
                    return;
                }
                else
                {
                    DateTime lastDisplayTime = RecipeNotificationHistory[recipe.Name];
                    if (DateTime.Now.Subtract(lastDisplayTime).TotalHours <= recipe.ScopeHours) //if already shown within scope-hours-time
                    {
                        Log.Write(string.Format("Ignoring Recipe '{0}' bcoz of scope {1} hours.", recipe.Name, recipe.ScopeHours));
                        return;
                    }
                    else
                    {
                        RecipeNotificationHistory.Remove(recipe.Name);
                    }
                }
            }

            new NotifyActivity().Execute(notificationArgs);
            RecipeNotificationHistory.Add(recipe.Name, DateTime.Now);
        }

        #endregion
    }
}
