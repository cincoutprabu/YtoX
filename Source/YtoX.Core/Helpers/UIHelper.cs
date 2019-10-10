//UIHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
using YtoX.Entities;

namespace YtoX.Core.Helpers
{
    public class UIHelper
    {
        #region Methods

        public static void ShowAlert(string text)
        {
            MessageBox.Show(text, Constants.APP_ID, MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //runs the action after specified duration
        public static void Run(Action action, long milliSeconds)
        {
            var timer = new DispatcherTimer();
            timer.Tick += delegate(object sender, EventArgs e)
            {
                timer.Stop();
                action.Invoke();
            };
            timer.Interval = TimeSpan.FromMilliseconds(milliSeconds);
            timer.Start();
        }

        #endregion
    }
}
