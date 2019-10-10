//TaskBarHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Drawing;
using System.ServiceProcess;
using System.Diagnostics;

using YtoX.Entities;
using YtoX.Core;

namespace YtoX
{
    public class TaskBarHelper
    {
        private static NotifyIcon SharedNotifyIcon;

        #region Methods

        public static void Setup()
        {
            try
            {
                //create menu-options
                ContextMenu contextMenu = new ContextMenu();

                MenuItem controlPanelOption = new MenuItem("Control Panel");
                controlPanelOption.Click += new EventHandler(controlPanelOption_Click);
                contextMenu.MenuItems.Add(controlPanelOption);

                MenuItem exitOption = new MenuItem("Exit");
                exitOption.Click += exitOption_Click;
                contextMenu.MenuItems.Add(exitOption);

                //create notify-icon
                SharedNotifyIcon = new System.Windows.Forms.NotifyIcon();
                SharedNotifyIcon.Text = "YtoX";
                var streamInfo = System.Windows.Application.GetResourceStream(new Uri("/Images/Logo.ico", UriKind.RelativeOrAbsolute));
                if (streamInfo != null)
                {
                    SharedNotifyIcon.Icon = new Icon(streamInfo.Stream);
                }
                SharedNotifyIcon.ContextMenu = contextMenu;
                SharedNotifyIcon.MouseDoubleClick += SharedNotifyIcon_MouseDoubleClick;

                //show notify-icon
                SharedNotifyIcon.Visible = true;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void ShowBalloonTip(string text)
        {
            SharedNotifyIcon.ShowBalloonTip(4000, "YtoX", text, ToolTipIcon.Info);
        }

        #endregion

        #region Events

        static void SharedNotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ControlPanel.Instance.ShowFromTray();
        }

        static void controlPanelOption_Click(object sender, EventArgs e)
        {
            ControlPanel.Instance.ShowFromTray();
        }

        static void exitOption_Click(object sender, EventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("All YtoX recipes will stop running. Quit now?", Constants.APP_ID, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Engine.Instance.Stop();
                App.Current.Shutdown();
            }
        }

        #endregion
    }
}
