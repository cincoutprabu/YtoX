//NotifyActivity.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

using YtoX.Core.Helpers;

namespace YtoX.Core.Activities
{
    public class NotifyActivity : IActivity
    {
        public static int TOAST_COUNTER = 0;

        #region Methods

        public void Execute(Dictionary<string, object> arguments)
        {
            string headerText = (string)arguments["HeaderText"];
            string alertText = (string)arguments["AlertText"];
            bool persistent = (bool)arguments["Persistent"];

            string toastId = (++TOAST_COUNTER).ToString();
            XmlDocument document = ToastHelper.BuildToastXml(toastId, headerText, alertText, persistent);

            if (arguments.ContainsKey("Delay"))
            {
                long delay = long.Parse(arguments["Delay"].ToString());
                ToastHelper.ScheduleToast(toastId, document, delay);
            }
            else
            {
                ToastHelper.CreateToast(document);
            }
        }

        #endregion
    }
}
