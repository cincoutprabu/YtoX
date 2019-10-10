//DataViewer.xaml.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Timers;
using System.Windows.Threading;

using YtoX.Entities;
using YtoX.Core;
using YtoX.Core.Storage;
using YtoX.Core.Weather;
using YtoX.Core.Location;
using YtoX.Core.News;
using YtoX.DataGrid;

namespace YtoX
{
    public partial class DataViewer : UserControl
    {
        #region Constructors

        public DataViewer()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(DataViewer_Loaded);
        }

        #endregion

        #region Control-Events

        void DataViewer_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void ClearWeatherHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Clear Weather History?", Constants.APP_ID, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                WeatherHistoryDAC.Clear();
                RefreshWeatherHistoryButton_Click(null, null);
            }
        }

        private void RefreshWeatherHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //create table
                StringTable table = new StringTable();
                table.ColumnNames.Add("Observed On");
                table.ColumnNames.Add("Location");
                table.ColumnNames.Add("Temperature");
                table.ColumnNames.Add("Forecast");

                //add rows
                var history = WeatherHistoryDAC.FetchAll();
                foreach (var entry in history)
                {
                    table.Add(table.Count, new StringRow() 
                    {
                        { "Observed On", entry.FetchedOn.ToString() },
                        { "Location", entry.Latitude + ", " + entry.Longitude },
                        { "Temperature", entry.Temperature == double.MaxValue ? string.Empty : entry.Temperature.ToString() },
                        { "Forecast", entry.Forecast.ToString() }
                    });
                }

                DataGridHelper.Bind(ref WeatherHistoryGrid, table);

                //show summary
                WeatherSummaryTextBox.Text = string.Format("{0} Records", history.Count);
            }
            catch (Exception exception)
            {
                WeatherSummaryTextBox.Text = "Unable to Load Weather-History: " + exception.Message;
            }
        }

        private void ClearLocationHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Some location-based recipes depend on historical location data. Proceed to clear the history?", Constants.APP_ID, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                LocationHistoryDAC.Clear();
                LocationHelper.History.Clear();
                RefreshLocationHistoryButton_Click(null, null);
            }
        }

        private void RefreshLocationHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //create table
                StringTable table = new StringTable();
                table.ColumnNames.Add("VisitedOn");
                table.ColumnNames.Add("Location");
                table.ColumnNames.Add("Known Location");

                //add rows
                foreach (var entry in LocationHelper.History)
                {
                    table.Add(table.Count, new StringRow() 
                    {
                        { "VisitedOn", entry.VisitedOn.ToString() },
                        { "Location", entry.Latitude + ", " + entry.Longitude },
                        { "Known Location", entry.KnownKey.ToString() }
                    });
                }

                DataGridHelper.Bind(ref LocationHistoryGrid, table);

                //show summary
                LocationSummaryTextBox.Text = string.Format("{0} Records", LocationHelper.History.Count);
            }
            catch (Exception exception)
            {
                LocationSummaryTextBox.Text = "Unable to Load Location-History: " + exception.Message;
            }
        }

        private void ClearNewsHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Clear News History?", Constants.APP_ID, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                NewsDAC.Clear();
                RefreshNewsHistoryButton_Click(null, null);
            }
        }

        private void RefreshNewsHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //create table
                StringTable table = new StringTable();
                table.ColumnNames.Add("Provider");
                table.ColumnNames.Add("Title");
                table.ColumnNames.Add("Url");
                table.ColumnNames.Add("Email");
                table.ColumnNames.Add("Email Sent");

                //add rows
                var allNews = NewsDAC.FetchAll();
                foreach (var entry in allNews)
                {
                    table.Add(table.Count, new StringRow() 
                    {
                        { "Provider", entry.Provider },
                        { "Title", entry.Title },
                        { "Url", entry.Url },
                        { "Email", entry.EmailId },
                        { "Email Sent", entry.EmailSent.ToString() }
                    });
                }

                DataGridHelper.Bind(ref NewsHistoryGrid, table);

                //show summary
                NewsSummaryTextBox.Text = string.Format("{0} Records", allNews.Count);
            }
            catch (Exception exception)
            {
                NewsSummaryTextBox.Text = "Unable to Load News-History: " + exception.Message;
            }
        }

        private void ClearLogButton_Click(object sender, RoutedEventArgs e)
        {
            Log.Clear();
            RefreshLogButton_Click(null, null);
        }

        private void RefreshLogButton_Click(object sender, RoutedEventArgs e)
        {
            LogTextBox.Text = Log.Read();
        }

        #endregion

        #region Internal-Methods

        private void LoadData()
        {
            RefreshWeatherHistoryButton_Click(null, null);
            RefreshLocationHistoryButton_Click(null, null);
            RefreshNewsHistoryButton_Click(null, null);
            RefreshLogButton_Click(null, null);
        }

        #endregion
    }
}
