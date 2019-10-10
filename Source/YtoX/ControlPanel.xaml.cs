//MainWindow.xaml.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

using YtoX.Entities;
using YtoX.Core;
using YtoX.Core.Helpers;
using YtoX.Core.Activities;

namespace YtoX
{
    public partial class ControlPanel : Window
    {
        public static ControlPanel Instance;

        #region Constructors

        public ControlPanel()
        {
            InitializeComponent();
            Instance = this;

            this.Loaded += ControlPanel_Loaded;
        }

        #endregion

        #region Methods

        public void ShowFromTray()
        {
            this.ShowInTaskbar = true;
            this.WindowState = WindowState.Normal;
            this.Show();
        }

        #endregion

        #region Control-Events

        void ControlPanel_Loaded(object sender, RoutedEventArgs e)
        {
            //TestbedButton.IsChecked = true;
            RecipesButton.IsChecked = true;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            e.Cancel = true;
            this.ShowInTaskbar = false;
            this.WindowState = WindowState.Minimized;
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.ShowInTaskbar = false;
            this.WindowState = WindowState.Minimized;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void WebButton_Click(object sender, RoutedEventArgs e)
        {
            new OpenUrlActivity().Execute(new Dictionary<string, object>() { { "Url", "http://www.ytox.net/" } });
        }

        private void TestbedButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!(MainContent.Content is Testbed))
            {
                OnButtonSelected(TestbedButton);
                MainContent.Content = new Testbed();
            }
        }

        private void RecipesButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!(MainContent.Content is RecipesViewer))
            {
                OnButtonSelected(RecipesButton);
                MainContent.Content = new RecipesViewer();
            }
        }

        private void ConfigButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!(MainContent.Content is Settings))
            {
                OnButtonSelected(ConfigButton);
                MainContent.Content = new Settings();
            }
        }

        private void AdvancedButton_Checked(object sender, RoutedEventArgs e)
        {
            if (!(MainContent.Content is DataViewer))
            {
                OnButtonSelected(AdvancedButton);
                MainContent.Content = new DataViewer();
            }
        }

        #endregion

        #region Internal-Methods

        private void OnButtonSelected(ToggleButton button)
        {
            var allButtons = (new ToggleButton[] { TestbedButton, RecipesButton, ConfigButton, AdvancedButton }).ToList();
            //allButtons.Remove(button);

            allButtons.ForEach(bt => { bt.Background = new SolidColorBrush(Colors.LightGray); bt.IsChecked = false; });
            button.Background = new SolidColorBrush(Colors.DarkGray);
        }

        #endregion
    }
}
