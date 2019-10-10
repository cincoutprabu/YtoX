//OutlookHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Reflection;

using Microsoft.Office.Core;
using Microsoft.Office.Interop.Outlook;

namespace YtoX.Core.Helpers
{
    public class OutlookHelper
    {
        private static NameSpace session;
        private static Items inboxItems;
        private static Items calendarItems;

        private static List<MailItem> unreadEmails;
        private static List<AppointmentItem> pendingAppointments;
        public static int InProgressMeetings = 0;

        private static Timer _calendarTimer;

        #region Methods

        public static void StartTracking()
        {
            try
            {
                Log.Write("Attaching to Outlook");

                Application app = new Application();
                session = app.GetNamespace("MAPI");
                session.Logon(Missing.Value, Missing.Value, false, false);

                //LoadEmails();
                LoadAppointments();

                Log.Write("Outlook Tracking Started");
            }
            catch (System.Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void StopTracking()
        {
            StopEvents();

            Log.Write("Outlook Tracking Stopped");
        }

        private static void StopEvents()
        {
            if (calendarItems != null)
            {
                calendarItems.ItemAdd -= calendarItems_ItemAdd;
                calendarItems.ItemChange -= calendarItems_ItemChange;
                calendarItems.ItemRemove -= calendarItems_ItemRemove;
            }

            if (pendingAppointments != null)
            {
                pendingAppointments.Clear();
            }

            if (_calendarTimer != null)
            {
                _calendarTimer.Stop();
            }

        }

        private static void LoadEmails()
        {
            var inbox = session.GetDefaultFolder(OlDefaultFolders.olFolderInbox);
            inboxItems = inbox.Items;

            if (inbox.UnReadItemCount > 0)
            {
                //first time, take unread-item-count from Inbox
                int previousUnreadCount = unreadEmails == null ? inbox.UnReadItemCount : unreadEmails.Count;

                unreadEmails = new List<MailItem>();
                foreach (var item in inboxItems)
                {
                    if (item is MailItem)
                    {
                        if (((MailItem)item).UnRead)
                        {
                            unreadEmails.Add((MailItem)item);
                        }
                    }
                }

                inboxItems.ItemAdd += inboxItems_ItemAdd;
                inboxItems.ItemChange += inboxItems_ItemChange;
                inboxItems.ItemRemove += inboxItems_ItemRemove;

                if (unreadEmails.Count > previousUnreadCount)
                {
                    //new unread emails
                    //execute corresponding action
                }
            }
        }

        private static void LoadAppointments()
        {
            StopEvents();

            var calendar = session.GetDefaultFolder(OlDefaultFolders.olFolderCalendar);
            calendarItems = calendar.Items;

            //get future appointments & meetings
            pendingAppointments = new List<AppointmentItem>();
            foreach (var item in calendarItems)
            {
                if (item is AppointmentItem)
                {
                    if (((AppointmentItem)item).Start >= DateTime.Now)
                    {
                        pendingAppointments.Add((AppointmentItem)item);
                    }
                }
            }

            calendarItems.ItemAdd += calendarItems_ItemAdd;
            calendarItems.ItemChange += calendarItems_ItemChange;
            calendarItems.ItemRemove += calendarItems_ItemRemove;

            if (pendingAppointments.Count > 0)
            {
                //setup calendar timer
                _calendarTimer = new Timer(60 * 1000); //1-minute
                _calendarTimer.Elapsed += _calendarTimer_Elapsed;
                _calendarTimer.AutoReset = true;
                _calendarTimer.Start();

                _calendarTimer_Elapsed(null, null); //execute once at first
            }
        }

        public static string GetFirstAppointmentInfo()
        {
            if (pendingAppointments != null && pendingAppointments.Count > 0)
            {
                //find appointment with least starting time
                AppointmentItem first = null;
                foreach (var item in pendingAppointments)
                {
                    if (item.Start > DateTime.Now) //ignore currently in-progress appointments
                    {
                        if (first == null) first = item;
                        else
                        {
                            if (first.Start > item.Start)
                            {
                                first = item;
                            }
                        }
                    }
                }

                return "@" + first.Start.Hour + ":" + first.Start.Minute + ". " + first.Subject;
            }

            return null;
        }

        #endregion

        #region Events

        static void _calendarTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (pendingAppointments != null && pendingAppointments.Count > 0)
            {
                InProgressMeetings = 0;
                foreach (AppointmentItem item in pendingAppointments)
                {
                    if (DateTime.Now >= item.Start && DateTime.Now <= item.End)
                    {
                        InProgressMeetings += 1;
                    }
                }
                Log.Write(string.Format("{0}/{1} meetings in-progress.", InProgressMeetings, pendingAppointments == null ? 0 : pendingAppointments.Count));

                if (InProgressMeetings > 0)
                {
                    Execution.ExecuteMuteRecipe(true); //mute
                    //Execution.ExecuteRecordRecipe(true); //start-record
                }
                else
                {
                    Execution.ExecuteMuteRecipe(false); //unmute
                    //Execution.ExecuteRecordRecipe(false); //stop record
                }
            }
        }

        static void calendarItems_ItemAdd(object item)
        {
            LoadAppointments();

            //var appointment = (AppointmentItem)item;
            //Log.Write("Calendar Item Added: " + appointment.Subject + "," + appointment.Location + "," + appointment.MeetingStatus + "," + appointment.Start + "," + appointment.End);
        }

        static void calendarItems_ItemChange(object item)
        {
            LoadAppointments();

            //var appointment = (AppointmentItem)item;
            //Log.Write("Calendar Item Modified: " + appointment.Subject + "," + appointment.Location + "," + appointment.MeetingStatus + "," + appointment.Start + "," + appointment.End);
        }

        static void calendarItems_ItemRemove()
        {
            LoadAppointments();

            //Log.Write("Calendar Item Removed");
        }

        static void inboxItems_ItemAdd(object Item)
        {
            LoadEmails();
        }

        static void inboxItems_ItemChange(object Item)
        {
            LoadEmails();
        }

        static void inboxItems_ItemRemove()
        {
            LoadEmails();
        }

        #endregion
    }
}
