//Testbed.xaml.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Data;
using System.ComponentModel;

using YtoX.Entities;
using YtoX.Core;
using YtoX.Core.Helpers;
using YtoX.Core.Activities;
using YtoX.Core.Location;
using YtoX.Core.Weather;
using YtoX.Core.News;
using YtoX.Core.Storage;

namespace YtoX
{
    public partial class Testbed : UserControl
    {
        #region Constructors

        public Testbed()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(Dashboard_Loaded);
        }

        #endregion

        #region Control-Events

        void Dashboard_Loaded(object sender, RoutedEventArgs e)
        {
            //SettingsHelper.LoadConstants(true);
        }

        private void TaskBarButton_Click(object sender, RoutedEventArgs e)
        {
            TaskBarHelper.Setup();
        }

        private void ShowToastButton_Click(object sender, RoutedEventArgs e)
        {
            new NotifyActivity().Execute(new Dictionary<string, object>() { { "HeaderText", "YtoX Alert?" }, { "AlertText", "You have a meeting at 2pm." }, { "Delay", 500 }, { "Persistent", true } });
        }

        private void AudioRecordButton_Click(object sender, RoutedEventArgs e)
        {
            //AudioRecordActivity.Instance.Execute(new Dictionary<string, object>() { { "Duration", 10000 } });

            AudioRecordActivity.Instance.Start();
            UIHelper.Run(() => AudioRecordActivity.Instance.Stop(), 5000);
        }

        private void PhotoCaptureButton_Click(object sender, RoutedEventArgs e)
        {
            VideoCaptureActivity.Instance.Execute(new Dictionary<string, object>() { { "PhotoOnly", true }, { "Duration", 8000 } });
        }

        private void MuteButton_Click(object sender, RoutedEventArgs e)
        {
            //float volume = SystemHelper.GetMasterVolume();
            //MessageBox.Show(volume.ToString());

            //bool isMute = SystemHelper.IsMute();
            //MessageBox.Show("IsMute: " + isMute);

            new MuteActivity().Execute(new Dictionary<string, object>() { });
        }

        private void BrightnessButton_Click(object sender, RoutedEventArgs e)
        {
            SystemHelper.SetBrightness(70);
        }

        private void BatteryButton_Click(object sender, RoutedEventArgs e)
        {
            BatteryHelper.StartTracking();
            //BatteryHelper.GetBatteryRemaining();
        }

        private void OpenUrlButton_Click(object sender, RoutedEventArgs e)
        {
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "http://www.msn.com" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "bingnews://search/?q=Windows%208" } });

            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "bingnews://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "bingfinance://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "bingweather://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "bingtravel://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "bingsearch://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "bingsports://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "bingmaps://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "xboxgames://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "wlpeople://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "mschat://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "microsoftvideo://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "microsoftmusic://" } });
            new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "ms-windows-store://" } });

            //ignored
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "onenote://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "mailto://" } });

            //not working
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "wlcalendar://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "netflix://" } });
            //new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "ebay://" } });
        }

        private void OpenAppButton_Click(object sender, RoutedEventArgs e)
        {
            new OpenAppActivity().Execute(new Dictionary<string, object>() { { "AppName", @"winword" } });
        }

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Images\Logo.png");
            new OpenFileActivity().Execute(new Dictionary<string, object>() { { "FilePath", filePath } });
        }

        private async void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            await LocationHelper.ReadLocation(true);
            Execution.ExecuteLocationRecipes();

            //double d0 = LocationHelper.GetDistance(KnownLocations.Get(KnownLocations.HOME), KnownLocations.Get(KnownLocations.HOME));
            //double d1 = LocationHelper.GetDistance(KnownLocations.Get(KnownLocations.HOME), KnownLocations.Get(KnownLocations.WORK));
            //double d2 = LocationHelper.GetDistance(KnownLocations.Get(KnownLocations.HOME), KnownLocations.Get(KnownLocations.GYM));
            //double d3 = LocationHelper.GetDistance(KnownLocations.Get(KnownLocations.HOME), KnownLocations.Get(KnownLocations.BIKE_SHOWROOM));
            //double d4 = LocationHelper.GetDistance(KnownLocations.Get(KnownLocations.HOME), KnownLocations.Get(KnownLocations.CAR_SHOWROOM));
        }

        private async void WeatherButton_Click(object sender, RoutedEventArgs e)
        {
            //double latitude = 12.984722, longitude = 80.251944; //Chennai Perungudi
            //double latitude = 40.7356, longitude = -74.1728; //Newark City

            //YahooWeather.GetWeather();
            //NDFDWeather.GetWeather(latitude, longitude);
            //WeatherDetails weatherDetails = WWO.GetWeather(latitude, longitude);

            //Position position = Wunderground.GeoLookup(latitude, longitude);
            //string forecast = Wunderground.GetForecast(latitude, longitude);

            //WeatherDetails weatherDetails = Wunderground.GetWeather(latitude, longitude);
            //string forecast = Wunderground.GetForecast(latitude, longitude);

            //float currentWeather = Wunderground.GetWeather(latitude, longitude);

            await Execution.ExecuteWeatherRecipes();
        }

        private void SensorButton_Click(object sender, RoutedEventArgs e)
        {
            SensorHelper.StartTracking();
        }

        private void SystemUptimeButton_Click(object sender, RoutedEventArgs e)
        {
            TimeSpan uptime = SystemHelper.GetSystemUpTime();
            //TimeSpan uptime = TimeSpan.FromMilliseconds(Environment.TickCount);

            string duration = string.Format("{0:%h} hours {0:%m} minutes", uptime);
            string duration1 = uptime.Days + " days " + uptime.Hours + " hours " + uptime.Minutes + " minutes " + uptime.Seconds + " seconds";
            MessageBox.Show(duration1);
        }

        private void OutlookButton_Click(object sender, RoutedEventArgs e)
        {
            OutlookHelper.StartTracking();
        }

        private void GTalkButton_Click(object sender, RoutedEventArgs e)
        {
            GTalkHelper.StartTracking();
        }

        private void SkypeButton_Click(object sender, RoutedEventArgs e)
        {
            SkypeHelper.StartTracking();
        }

        private void FacebookButton_Click(object sender, RoutedEventArgs e)
        {
            FacebookHelper.StartTracking();
        }

        private void TwitterButton_Click(object sender, RoutedEventArgs e)
        {
            TwitterHelper.PostTweet();
        }

        private void DropboxButton_Click(object sender, RoutedEventArgs e)
        {
            DropboxHelper.StartTracking();
        }

        private void NewsButton_Click(object sender, RoutedEventArgs e)
        {
            //NewsHelper.ReadNews();
            Execution.ExecuteNewsRecipes();
        }

        private void LambdaButton_Click(object sender, RoutedEventArgs e)
        {
            Repository.GetWeatherRecipe();
        }

        private void StorageButton_Click(object sender, RoutedEventArgs e)
        {
            using (DAC dac = new DAC())
            {
                //
            }
        }

        private void LifestyleButton_Click(object sender, RoutedEventArgs e)
        {
            Execution.ExecuteLifestyleRecipes();
        }

        #endregion
    }
}
