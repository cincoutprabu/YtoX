//ToastHelper.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

using YtoX.Entities;

namespace YtoX.Core.Helpers
{
    public class NotificationItem
    {
        #region Properties

        public string Id { get; set; }
        public string DueTime { get; set; }
        public string Text { get; set; }

        #endregion
    }

    public class ToastHelper
    {
        #region Methods

        public static XmlDocument BuildToastXml(string id, string headerText, string alertText, bool persistentAudio)
        {
            string logoPath = "file:///" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Images\Logo-Toast.png");
            string audioSrc = persistentAudio ? "Notification.Looping.Alarm2" : "Notification.Reminder";

            string toastXml = string.Format(@"
<toast launch='toast_{0}' duration='{1}'>
    <visual version='1'>
        <binding template='ToastImageAndText02'>
            <image id='1' src='{2}' alt='Logo'></image>
            <text id='1'>{3}</text>
            <text id='2'>{4}</text>
        </binding>
    </visual>
    <audio src='ms-winsoundevent:{5}' loop='{6}' />
</toast>", id, persistentAudio ? "long" : "short", logoPath, headerText, alertText, audioSrc, persistentAudio.ToString().ToLower());

            XmlDocument document = new XmlDocument();
            document.LoadXml(toastXml);
            return document;
        }

        public static void CreateToast(XmlDocument document)
        {
            ToastNotification toast = new ToastNotification(document);
            toast.Failed += toast_Failed;
            toast.Activated += toast_Activated;
            toast.Dismissed += toast_Dismissed;
            ToastNotificationManager.CreateToastNotifier(Constants.APP_ID).Show(toast);
        }

        static void toast_Failed(ToastNotification sender, ToastFailedEventArgs args)
        {
            //
        }

        static void toast_Activated(ToastNotification sender, object args)
        {
            //UIHelper.ShowAlert("@toast_Activated");
        }

        static void toast_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            //
        }

        public static void ScheduleToast(string toastId, XmlDocument document, long milliSeconds)
        {
            DateTime dueTime = DateTime.Now.AddMilliseconds(milliSeconds);
            var scheduledToast = new ScheduledToastNotification(document, dueTime);
            scheduledToast.Id = toastId;
            ToastNotificationManager.CreateToastNotifier(Constants.APP_ID).AddToSchedule(scheduledToast);
        }

        public static void UnscheduleToast(NotificationItem item)
        {
            var notifier = ToastNotificationManager.CreateToastNotifier(Constants.APP_ID);
            var scheduledToasts = notifier.GetScheduledToastNotifications();

            for (int i = 0; i < scheduledToasts.Count; i++)
            {
                if (scheduledToasts[i].Id == item.Id)
                {
                    notifier.RemoveFromSchedule(scheduledToasts[i]);
                }
            }
        }

        public static List<NotificationItem> GetAllNotifications()
        {
            var scheduledToasts = ToastNotificationManager.CreateToastNotifier(Constants.APP_ID).GetScheduledToastNotifications();

            List<NotificationItem> notificationList = new List<NotificationItem>();
            for (int i = 0; i < scheduledToasts.Count; i++)
            {
                ScheduledToastNotification toast = scheduledToasts[i];

                notificationList.Add(new NotificationItem()
                {
                    Id = toast.Id,
                    DueTime = toast.DeliveryTime.ToLocalTime().ToString(),
                    Text = toast.Content.GetElementsByTagName("text")[0].InnerText
                });
            }

            return notificationList;
        }

        #endregion
    }
}
