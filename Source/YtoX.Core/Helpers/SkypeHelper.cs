//SkypeHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SKYPE4COMLib;

using YtoX.Entities;

namespace YtoX.Core.Helpers
{
    public class SkypeHelper
    {
        private static Skype skype = null;

        #region Methods

        public static void StartTracking()
        {
            try
            {
                Log.Write("Skype Enabled: " + Constants.SKYPE_ENABLED);

                skype = new Skype();
                Log.Write("Skype Running: " + skype.Client.IsRunning);
                if (!skype.Client.IsRunning)
                {
                    skype.Client.Start(true, true);
                }

                skype.Attach(7, true);
                Log.Write("Attached to Skype");
                skype.MessageStatus += skype_MessageStatus;

                Log.Write("Skype Tracking Started");
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void StopTracking()
        {
            if (skype != null)
            {
                skype.MessageStatus -= skype_MessageStatus;
                skype = null;

                Log.Write("Skype Tracking Stopped");
            }
        }

        #endregion

        #region Events

        static void skype_MessageStatus(ChatMessage message, TChatMessageStatus status)
        {
            if (!Constants.SKYPE_ENABLED) return;

            try
            {
                string from = message.Sender.Handle;
                Log.Write("Skype: New chat from '" + from + "' (" + status + "): " + message.Body);

                switch (status)
                {
                    case TChatMessageStatus.cmsReceived:
                        {
                            var autoReplyAction = new Action<string>(text =>
                            {
                                skype.SendMessage(from, text);
                            });
                            Execution.ExecuteBusyRecipe("Skype", from, autoReplyAction);
                            Execution.ExecuteBatteryLowRecipe("Skype", from, autoReplyAction);
                        }
                        break;
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion
    }
}
