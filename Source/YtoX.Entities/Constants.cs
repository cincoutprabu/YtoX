//Constants.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YtoX.Entities
{
    public class Constants
    {
        #region Constants

        public const string APP_ID = "YtoX";
        public const string ENGINE_ID = "YtoXEngine";
        public const string LOG_FILENAME = "YtoX-Log.txt";
        public const string LOG_DATE_FORMAT = "yyyy-MMM-dd HH:mm:ss";
        public const string FILENAME_DATE_FORMAT = "yyyy_MMM_dd HH_mm_ss";
        public const int GRID_REFRESH_INTERVAL = 20;

        public static string MS_TOP = "Microsoft Top Stories";
        public static string GOOGLE_TECH = "Google Technology";
        public static string YAHOO_TECH = "Yahoo Technology";
        public static string BBC_TECH = "BBC Technology";
        public static Dictionary<string, string> AllNewsProviders = new Dictionary<string, string>() 
        {
            {MS_TOP, "http://www.microsoft.com/en-us/news/rss/rssfeed.aspx?ContentType=FeatureStories"},
            {GOOGLE_TECH, "http://news.google.com/news?pz=1&cf=all&ned=us&hl=en&topic=tc&output=rss"},
            {YAHOO_TECH, "http://news.yahoo.com/rss/tech"},
            {BBC_TECH, "http://feeds.bbci.co.uk/news/technology/rss.xml"}
        };
        public static Dictionary<string, string> NewsProviderLinks = new Dictionary<string, string>()
        {
            {MS_TOP, "http://www.microsoft.com/news/rss/default.aspx"},
            {GOOGLE_TECH, "http://news.google.com/news?pz=1&cf=all&ned=us&hl=en&topic=tc"},
            {YAHOO_TECH, "http://news.yahoo.com/tech/"},
            {BBC_TECH, "http://www.bbc.com/news/technology/"}
        };

        public static string[] DayOfWeeks = { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
        public static Dictionary<string, int> TimesOfDay = new Dictionary<string, int>()
        {
            {"12 Midnight", 0},
            {"1 am", 1},
            {"2 am", 2},
            {"3 am", 3},
            {"4 am", 4},
            {"5 am", 5},
            {"6 am", 6},
            {"7 am", 7},
            {"8 am", 8},
            {"9 am", 9},
            {"10 am", 10},
            {"11 am", 11},
            {"12 Noon", 12},
            {"1 pm", 13},
            {"2 pm", 14},
            {"3 pm", 15},
            {"4 pm", 16},
            {"5 pm", 17},
            {"6 pm", 18},
            {"7 pm", 19},
            {"8 pm", 20},
            {"9 pm", 21},
            {"10 pm", 22},
            {"11 pm", 23}
        };

        public static Dictionary<string, string> AppUrls = new Dictionary<string, string>()
        {
            {"Finance", "bingfinance://"},
            {"Games", "xboxgames://"},
            {"Maps", "bingmaps://"},
            {"Messaging", "mschat://"},
            {"Music", "microsoftmusic://"},
            {"News", "bingnews://"},
            {"People", "wlpeople://"},
            {"Search", "bingsearch://"},
            {"Sports", "bingsports://"},
            {"Store", "ms-windows-store://"},
            {"Travel", "bingtravel://"},
            {"Video", "microsoftvideo://"},
            {"Weather", "bingweather://"},
        };

        public static Dictionary<string, string> AppComments = new Dictionary<string, string>()
        {
            {"Finance", "the market"},
            {"Games", "Games"},
            {"Maps", "Maps"},
            {"Messaging", "Messaging"},
            {"Music", "Music"},
            {"News", "News"},
            {"People", "all my family & friends"},
            {"Search", "Search"},
            {"Sports", "Sports"},
            {"Store", "Store"},
            {"Travel", "Travel"},
            {"Video", "Videos"},
            {"Weather", "Weather"},
        };

        public static string WORD = "WORD";
        public static string EXCEL = "EXCEL";
        public static string POWERPOINT = "POWERPOINT";
        public static Dictionary<string, string[]> DocumentFilters = new Dictionary<string, string[]>()
        {
            {WORD, new string[] {"*.doc", "*.docx"}},
            {EXCEL, new string[] {"*.xls", "*.xlsx"}},
            {POWERPOINT, new string[] {"*.ppt", "*.pptx"}},
        };

        #endregion

        #region Settings

        //engine
        public static bool ENGINE_ENABLED = true;
        public static int ENGINE_INTERVAL = 60; //seconds
        public static int APP_USAGE_COUNT = 0;

        //weather
        public static int WEATHER_READ_INTERVAL = 60; //minutes
        public static double TEMPERATURE_ALERT_THRESHOLD = 20.0; //degree Celcius

        //location
        public static double VISITED_DISTANCE_THRESHOLD = 0.5;
        public static int LEFT_FROM_DURATION_THRESHOLD = 60; //minutes
        public static int CAR_SERVICE_INTERVAL = 90; //days
        public static int MAX_GYM_VACATION = 3; //days

        //network
        public static string NEWS_EMAIL = "";
        public static int NEWS_READ_INTERVAL = 120; //minutes
        public static string NEWS_PROVIDERS = "Google Technology;Yahoo Technology";
        public static string NEWS_TOPICS = "Windows 8, Windows Phone";
        public static string NEWS_PEOPLE = "Ballmer, Sinofsky";

        //social
        public static bool GTALK_ENABLED = true;
        public static string GTALK_USERNAME = "";
        public static string GTALK_PWD = "";
        public static bool SKYPE_ENABLED = true;
        public static string MEETING_AUTOREPLY_MESSAGE = "AutoReply";
        public static string BATTERYLOW_AUTOREPLY_MESSAGE = "AutoReply";
        public static int BATTERY_WARNING_THRESHOLD = 20;

        //lifestyle
        public static string MEETING_ALERT_HOUR = "10 pm";
        public static string TIMESHEET_ALERT_HOUR = "6 pm";
        public static string WEEKLY_SUMMARY_ALERT_DAY = "Friday";
        public static string WEEKLY_SUMMARY_ALERT_HOUR = "6 pm";
        public static string APP_TO_LAUNCH_ON_SHAKE = "Photos";
        public static string APP_TO_LAUNCH_ON_LANDSCAPE = "People";
        public static string DROPBOX_DOCUMENTS = "WORD;EXCEL";

        #endregion
    }
}
