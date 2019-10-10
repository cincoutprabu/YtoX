//Settings.xaml.cs

using System;
using System.IO;
using System.IO.Ports;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Web;

using YtoX.Entities;
using YtoX.Core;
using YtoX.Core.Helpers;
using YtoX.Core.Location;
using YtoX.Core.Storage;
using YtoX.Core.Activities;

namespace YtoX
{
    public partial class Settings : UserControl
    {
        public const string LocationDistanceHelpText = "If distance between the user's location and a known-location is less than this value, then the user will be considered to have visited that known-location.";
        public const string LocationDurationHelpText = "If the time since user has visited a location is less than this value, then the user will be considered to have left from that location recently.";

        #region Constructors

        public Settings()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Settings_Loaded);
        }

        #endregion

        #region Control-Events

        void Settings_Loaded(object sender, RoutedEventArgs e)
        {
            //engine
            EngineIntervalUpDown.Value = Constants.ENGINE_INTERVAL;
            UpdateEngineStatus();

            //weather
            WeatherReadIntervalUpDown.Value = Constants.WEATHER_READ_INTERVAL;
            TemperatureAlertThresholdUpDown.Value = Constants.TEMPERATURE_ALERT_THRESHOLD;

            //location
            VisitedDistanceThresholdUpDown.Value = Constants.VISITED_DISTANCE_THRESHOLD;
            LeftFromDurationUpDown.Value = Constants.LEFT_FROM_DURATION_THRESHOLD;
            CarServiceIntervalUpDown.Value = Constants.CAR_SERVICE_INTERVAL;
            MaxGymVacationUpDown.Value = Constants.MAX_GYM_VACATION;

            //network
            NewsEmailTextBox.Text = Constants.NEWS_EMAIL;
            NewsReadIntervalUpDown.Value = Constants.NEWS_READ_INTERVAL;
            NewsTopicsTextBox.Text = Constants.NEWS_TOPICS;
            NewsPeopleTextBox.Text = Constants.NEWS_PEOPLE;

            string[] providers = Constants.NEWS_PROVIDERS.Split(';');
            MSTopCheckBox.IsChecked = providers.Contains(Constants.MS_TOP);
            GoogleTechCheckBox.IsChecked = providers.Contains(Constants.GOOGLE_TECH);
            YahooTechCheckBox.IsChecked = providers.Contains(Constants.YAHOO_TECH);
            BbcTechCheckBox.IsChecked = providers.Contains(Constants.BBC_TECH);

            //social
            GTalkCheckBox.IsChecked = Constants.GTALK_ENABLED;
            GTalkEmailTextBox.Text = Constants.GTALK_USERNAME;
            GTalkPwdTextBox.Password = "000000";
            SkypeCheckBox.IsChecked = Constants.SKYPE_ENABLED;
            MeetingAutoReplyTextBox.Text = Constants.MEETING_AUTOREPLY_MESSAGE;
            BatteryLowAutoReplyTextBox.Text = Constants.BATTERYLOW_AUTOREPLY_MESSAGE;
            BatteryWarningThresholdUpDown.Value = Constants.BATTERY_WARNING_THRESHOLD;

            //lifestyle
            MeetingAlertHourComboBox.ItemsSource = Constants.TimesOfDay.Keys.ToList();
            TimeSheetAlertHourComboBox.ItemsSource = Constants.TimesOfDay.Keys.ToList();
            WeeklySummaryDayOfWeekComboBox.ItemsSource = Constants.DayOfWeeks;
            WeeklySummaryAlertHourComboBox.ItemsSource = Constants.TimesOfDay.Keys.ToList();
            LaunchOnShakeComboBox.ItemsSource = Constants.AppUrls.Keys.ToList();
            LaunchOnLandscapeComboBox.ItemsSource = Constants.AppUrls.Keys.ToList();

            MeetingAlertHourComboBox.SelectedItem = Constants.MEETING_ALERT_HOUR;
            TimeSheetAlertHourComboBox.SelectedItem = Constants.TIMESHEET_ALERT_HOUR;
            WeeklySummaryDayOfWeekComboBox.SelectedItem = Constants.WEEKLY_SUMMARY_ALERT_DAY;
            WeeklySummaryAlertHourComboBox.SelectedItem = Constants.WEEKLY_SUMMARY_ALERT_HOUR;
            LaunchOnShakeComboBox.SelectedItem = Constants.APP_TO_LAUNCH_ON_SHAKE;
            LaunchOnLandscapeComboBox.SelectedItem = Constants.APP_TO_LAUNCH_ON_LANDSCAPE;

            string[] docTypes = Constants.DROPBOX_DOCUMENTS.Split(';');
            WordCheckBox.IsChecked = docTypes.Contains(Constants.WORD);
            ExcelCheckBox.IsChecked = docTypes.Contains(Constants.EXCEL);
            PowerPointCheckBox.IsChecked = docTypes.Contains(Constants.POWERPOINT);
        }

        private void EngineButton_Click(object sender, RoutedEventArgs e)
        {
            string text = EngineButton.Content.ToString();
            if (text.Contains("Start"))
            {
                Engine.Instance.Start();
                EngineButton.Content = "Stop Engine";
            }
            else
            {
                Engine.Instance.Stop();
                EngineButton.Content = "Start Engine";
            }
            UpdateEngineStatus();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateEngineStatus();
        }

        private void DistanceHelpText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.Content = LocationDistanceHelpText;
            toolTip.IsOpen = true;

            UIHelper.Run(() => toolTip.IsOpen = false, 4000);
        }

        private void DurationHelpText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ToolTip toolTip = new ToolTip();
            toolTip.Content = LocationDurationHelpText;
            toolTip.IsOpen = true;

            UIHelper.Run(() => toolTip.IsOpen = false, 4000);
        }

        private void MSTopButton_Click(object sender, RoutedEventArgs e)
        {
            new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", Constants.NewsProviderLinks[Constants.MS_TOP] } });
        }

        private void GoogleTechButton_Click(object sender, RoutedEventArgs e)
        {
            new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", Constants.NewsProviderLinks[Constants.GOOGLE_TECH] } });
        }

        private void YahooTechButton_Click(object sender, RoutedEventArgs e)
        {
            new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", Constants.NewsProviderLinks[Constants.YAHOO_TECH] } });
        }

        private void BbcTechButton_Click(object sender, RoutedEventArgs e)
        {
            new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", Constants.NewsProviderLinks[Constants.BBC_TECH] } });
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveButton.IsEnabled = false;

                //engine
                Constants.ENGINE_INTERVAL = (int)EngineIntervalUpDown.Value;

                //weather
                Constants.WEATHER_READ_INTERVAL = (int)WeatherReadIntervalUpDown.Value;
                Constants.TEMPERATURE_ALERT_THRESHOLD = (double)TemperatureAlertThresholdUpDown.Value;

                //location
                Constants.VISITED_DISTANCE_THRESHOLD = (double)VisitedDistanceThresholdUpDown.Value;
                Constants.LEFT_FROM_DURATION_THRESHOLD = (int)LeftFromDurationUpDown.Value;
                Constants.CAR_SERVICE_INTERVAL = (int)CarServiceIntervalUpDown.Value;
                Constants.MAX_GYM_VACATION = (int)MaxGymVacationUpDown.Value;

                //network
                Constants.NEWS_EMAIL = NewsEmailTextBox.Text;
                Constants.NEWS_READ_INTERVAL = (int)NewsReadIntervalUpDown.Value;
                Constants.NEWS_TOPICS = NewsTopicsTextBox.Text;
                Constants.NEWS_PEOPLE = NewsPeopleTextBox.Text;

                List<string> providers = new List<string>();
                if ((bool)MSTopCheckBox.IsChecked) providers.Add(Constants.MS_TOP);
                if ((bool)GoogleTechCheckBox.IsChecked) providers.Add(Constants.GOOGLE_TECH);
                if ((bool)YahooTechCheckBox.IsChecked) providers.Add(Constants.YAHOO_TECH);
                if ((bool)BbcTechCheckBox.IsChecked) providers.Add(Constants.BBC_TECH);
                Constants.NEWS_PROVIDERS = string.Join(";", providers);

                //social
                Constants.GTALK_ENABLED = (bool)GTalkCheckBox.IsChecked;
                Constants.GTALK_USERNAME = GTalkEmailTextBox.Text;
                Constants.GTALK_PWD = new SimpleAES().EncryptToString(GTalkPwdTextBox.Password);
                Constants.SKYPE_ENABLED = (bool)SkypeCheckBox.IsChecked;
                Constants.MEETING_AUTOREPLY_MESSAGE = MeetingAutoReplyTextBox.Text;
                Constants.BATTERYLOW_AUTOREPLY_MESSAGE = BatteryLowAutoReplyTextBox.Text;
                Constants.BATTERY_WARNING_THRESHOLD = (int)BatteryWarningThresholdUpDown.Value;

                //lifestyle
                Constants.MEETING_ALERT_HOUR = MeetingAlertHourComboBox.SelectedItem.ToString();
                Constants.TIMESHEET_ALERT_HOUR = TimeSheetAlertHourComboBox.SelectedItem.ToString();
                Constants.WEEKLY_SUMMARY_ALERT_DAY = WeeklySummaryDayOfWeekComboBox.SelectedItem.ToString();
                Constants.WEEKLY_SUMMARY_ALERT_HOUR = WeeklySummaryAlertHourComboBox.SelectedItem.ToString();
                Constants.APP_TO_LAUNCH_ON_SHAKE = LaunchOnShakeComboBox.SelectedItem.ToString();
                Constants.APP_TO_LAUNCH_ON_LANDSCAPE = LaunchOnLandscapeComboBox.SelectedItem.ToString();

                List<string> docTypes = new List<string>();
                if ((bool)WordCheckBox.IsChecked) docTypes.Add(Constants.WORD);
                if ((bool)ExcelCheckBox.IsChecked) docTypes.Add(Constants.EXCEL);
                if ((bool)PowerPointCheckBox.IsChecked) docTypes.Add(Constants.POWERPOINT);
                Constants.DROPBOX_DOCUMENTS = string.Join(";", docTypes);

                bool restartSuccessful = false;
                Task.Run(() =>
                {
                    //engine
                    SettingsDAC.Set("ENGINE_ENABLED", Constants.ENGINE_ENABLED.ToString());
                    SettingsDAC.Set("ENGINE_INTERVAL", Constants.ENGINE_INTERVAL.ToString());

                    //weather
                    SettingsDAC.Set("WEATHER_READ_INTERVAL", Constants.WEATHER_READ_INTERVAL.ToString());
                    SettingsDAC.Set("TEMPERATURE_ALERT_THRESHOLD", Constants.TEMPERATURE_ALERT_THRESHOLD.ToString());

                    //location
                    SettingsDAC.Set("VISITED_DISTANCE_THRESHOLD", Constants.VISITED_DISTANCE_THRESHOLD.ToString());
                    SettingsDAC.Set("LEFT_FROM_DURATION_THRESHOLD", Constants.LEFT_FROM_DURATION_THRESHOLD.ToString());
                    SettingsDAC.Set("CAR_SERVICE_INTERVAL", Constants.CAR_SERVICE_INTERVAL.ToString());
                    SettingsDAC.Set("MAX_GYM_VACATION", Constants.MAX_GYM_VACATION.ToString());

                    //network
                    SettingsDAC.Set("NEWS_EMAIL", Constants.NEWS_EMAIL);
                    SettingsDAC.Set("NEWS_READ_INTERVAL", Constants.NEWS_READ_INTERVAL.ToString());
                    SettingsDAC.Set("NEWS_TOPICS", Constants.NEWS_TOPICS);
                    SettingsDAC.Set("NEWS_PEOPLE", Constants.NEWS_PEOPLE);
                    SettingsDAC.Set("NEWS_PROVIDERS", Constants.NEWS_PROVIDERS);

                    //social
                    SettingsDAC.Set("GTALK_ENABLED", Constants.GTALK_ENABLED.ToString());
                    SettingsDAC.Set("GTALK_USERNAME", Constants.GTALK_USERNAME);
                    SettingsDAC.Set("GTALK_PWD", Constants.GTALK_PWD);
                    SettingsDAC.Set("SKYPE_ENABLED", Constants.SKYPE_ENABLED.ToString());
                    SettingsDAC.Set("MEETING_AUTOREPLY_MESSAGE", Constants.MEETING_AUTOREPLY_MESSAGE);
                    SettingsDAC.Set("BATTERYLOW_AUTOREPLY_MESSAGE", Constants.BATTERYLOW_AUTOREPLY_MESSAGE);
                    SettingsDAC.Set("BATTERY_WARNING_THRESHOLD", Constants.BATTERY_WARNING_THRESHOLD.ToString());

                    //lifestyle
                    SettingsDAC.Set("MEETING_ALERT_HOUR", Constants.MEETING_ALERT_HOUR);
                    SettingsDAC.Set("TIMESHEET_ALERT_HOUR", Constants.TIMESHEET_ALERT_HOUR);
                    SettingsDAC.Set("WEEKLY_SUMMARY_ALERT_DAY", Constants.WEEKLY_SUMMARY_ALERT_DAY);
                    SettingsDAC.Set("WEEKLY_SUMMARY_ALERT_HOUR", Constants.WEEKLY_SUMMARY_ALERT_HOUR);
                    SettingsDAC.Set("APP_TO_LAUNCH_ON_SHAKE", Constants.APP_TO_LAUNCH_ON_SHAKE);
                    SettingsDAC.Set("APP_TO_LAUNCH_ON_LANDSCAPE", Constants.APP_TO_LAUNCH_ON_LANDSCAPE);
                    SettingsDAC.Set("DROPBOX_DOCUMENTS", Constants.DROPBOX_DOCUMENTS);

                    //save locations
                    KnownLocations.Save();

                    //restart engine
                    //Engine.Instance.Stop();
                    //await Engine.Instance.Start();
                    restartSuccessful = true;
                    //Log.Write("Engine Restarted");
                });
                Log.Write("Settings Saved Successfully");

                string statusMessage = string.Empty;
                if (restartSuccessful)
                {
                    statusMessage += "Settings saved successfully!";
                }
                else
                {
                    statusMessage = "Settings saved, but unable to restart the Engine. You have to do it manually, in order for new settings to take effect.";
                }
                MessageBox.Show(statusMessage, Constants.APP_ID, MessageBoxButton.OK, MessageBoxImage.Information);

                Engine.Instance.Stop();
                Engine.Instance.Start();
                SaveButton.IsEnabled = true;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                MessageBox.Show("Error: " + exception.Message, Constants.APP_ID, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            DoChooseLocation(KnownLocations.Get(KnownLocations.HOME));
        }

        private void WorkButton_Click(object sender, RoutedEventArgs e)
        {
            DoChooseLocation(KnownLocations.Get(KnownLocations.WORK));
        }

        private void GymButton_MouseDown(object sender, RoutedEventArgs e)
        {
            DoChooseLocation(KnownLocations.Get(KnownLocations.GYM));
        }

        private void BikeShowroomButton_Click(object sender, RoutedEventArgs e)
        {
            DoChooseLocation(KnownLocations.Get(KnownLocations.BIKE_SHOWROOM));
        }

        private void CarShowroomButton_Click(object sender, RoutedEventArgs e)
        {
            DoChooseLocation(KnownLocations.Get(KnownLocations.CAR_SHOWROOM));
        }

        #endregion

        #region Internal-Methods

        private void UpdateEngineStatus()
        {
            bool isRunning = Engine.Instance.IsRunning();
            EngineRunningTextBox.Text = isRunning ? "Yes" : "No";
            EngineButton.Content = isRunning ? "Stop Engine" : "Start Engine";
        }

        private void DoChooseLocation(Position pos)
        {
            var mapWindow = new MapWindow(pos);
            mapWindow.Title = pos.Key + " Location (double click to choose)";
            mapWindow.Chosen += mapWindow_Chosen;
            mapWindow.ShowDialog();
        }

        void mapWindow_Chosen(Position obj)
        {
            KnownLocations.Set(obj);
        }

        #endregion
    }
}
