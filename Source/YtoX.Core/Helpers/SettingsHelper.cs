//SettingsHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YtoX.Entities;
using YtoX.Core.Storage;
using YtoX.Core.Location;

namespace YtoX.Core.Helpers
{
    public class SettingsHelper
    {
        #region Methods

        public static void Load()
        {
            try
            {
                //engine
                Constants.ENGINE_ENABLED = Convert.ToBoolean(SettingsDAC.Get("ENGINE_ENABLED"));
                Constants.ENGINE_INTERVAL = Convert.ToInt32(SettingsDAC.Get("ENGINE_INTERVAL"));
                Constants.APP_USAGE_COUNT = Convert.ToInt32(SettingsDAC.Get("APP_USAGE_COUNT"));
                SettingsDAC.Set("APP_USAGE_COUNT", (Constants.APP_USAGE_COUNT + 1).ToString());

                //weather
                Constants.WEATHER_READ_INTERVAL = Convert.ToInt32(SettingsDAC.Get("WEATHER_READ_INTERVAL"));
                Constants.TEMPERATURE_ALERT_THRESHOLD = Convert.ToDouble(SettingsDAC.Get("TEMPERATURE_ALERT_THRESHOLD"));

                //location
                Constants.VISITED_DISTANCE_THRESHOLD = Convert.ToDouble(SettingsDAC.Get("VISITED_DISTANCE_THRESHOLD"));
                Constants.LEFT_FROM_DURATION_THRESHOLD = Convert.ToInt32(SettingsDAC.Get("LEFT_FROM_DURATION_THRESHOLD"));
                Constants.CAR_SERVICE_INTERVAL = Convert.ToInt32(SettingsDAC.Get("CAR_SERVICE_INTERVAL"));
                Constants.MAX_GYM_VACATION = Convert.ToInt32(SettingsDAC.Get("MAX_GYM_VACATION"));

                //network
                Constants.NEWS_EMAIL = SettingsDAC.Get("NEWS_EMAIL");
                Constants.NEWS_READ_INTERVAL = Convert.ToInt32(SettingsDAC.Get("NEWS_READ_INTERVAL"));
                Constants.NEWS_PROVIDERS = SettingsDAC.Get("NEWS_PROVIDERS");
                Constants.NEWS_TOPICS = SettingsDAC.Get("NEWS_TOPICS");
                Constants.NEWS_PEOPLE = SettingsDAC.Get("NEWS_PEOPLE");

                //social
                Constants.GTALK_ENABLED = Convert.ToBoolean(SettingsDAC.Get("GTALK_ENABLED"));
                Constants.GTALK_USERNAME = SettingsDAC.Get("GTALK_USERNAME");
                Constants.GTALK_PWD = SettingsDAC.Get("GTALK_PWD");
                Constants.SKYPE_ENABLED = Convert.ToBoolean(SettingsDAC.Get("SKYPE_ENABLED"));
                Constants.MEETING_AUTOREPLY_MESSAGE = SettingsDAC.Get("MEETING_AUTOREPLY_MESSAGE");
                Constants.BATTERYLOW_AUTOREPLY_MESSAGE = SettingsDAC.Get("BATTERYLOW_AUTOREPLY_MESSAGE");
                Constants.BATTERY_WARNING_THRESHOLD = Convert.ToInt32(SettingsDAC.Get("BATTERY_WARNING_THRESHOLD"));

                //lifestyle
                Constants.MEETING_ALERT_HOUR = SettingsDAC.Get("MEETING_ALERT_HOUR");
                Constants.TIMESHEET_ALERT_HOUR = SettingsDAC.Get("TIMESHEET_ALERT_HOUR");
                Constants.WEEKLY_SUMMARY_ALERT_DAY = SettingsDAC.Get("WEEKLY_SUMMARY_ALERT_DAY");
                Constants.WEEKLY_SUMMARY_ALERT_HOUR = SettingsDAC.Get("WEEKLY_SUMMARY_ALERT_HOUR");
                Constants.APP_TO_LAUNCH_ON_SHAKE = SettingsDAC.Get("APP_TO_LAUNCH_ON_SHAKE");
                Constants.APP_TO_LAUNCH_ON_LANDSCAPE = SettingsDAC.Get("APP_TO_LAUNCH_ON_LANDSCAPE");
                Constants.DROPBOX_DOCUMENTS = SettingsDAC.Get("DROPBOX_DOCUMENTS");

                //load known-locations
                KnownLocations.Load();
                LocationHelper.LoadHistory();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static string Encrypt_OneWay(string text)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(text);

            System.Security.Cryptography.SHA1 sha = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] cipherBytes = sha.ComputeHash(inputBytes);
            string cipher = Encoding.ASCII.GetString(cipherBytes);
            return cipher;
        }

        #endregion
    }
}
