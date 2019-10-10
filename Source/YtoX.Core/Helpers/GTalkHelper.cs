//GTalkHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using agsXMPP;
using agsXMPP.protocol;
using agsXMPP.protocol.client;
using agsXMPP.Xml;
using agsXMPP.Xml.Dom;

using YtoX.Entities;

namespace YtoX.Core.Helpers
{
    public class GTalkHelper
    {
        private static XmppClientConnection client = null;

        #region Methods

        public static void StartTracking()
        {
            Log.Write("GTalk Enabled: " + Constants.GTALK_ENABLED);

            if (string.IsNullOrEmpty(Constants.GTALK_USERNAME) || string.IsNullOrEmpty(Constants.GTALK_PWD))
            {
                Log.Write("Missing GTalk Username/Password.");
                return;
            }

            try
            {
                Jid jabberId = new Jid(Constants.GTALK_USERNAME);

                client = new XmppClientConnection();
                client.Password = new SimpleAES().DecryptString(Constants.GTALK_PWD);
                client.Username = jabberId.User;
                client.Server = jabberId.Server;
                client.AutoResolveConnectServer = true;

                client.OnAuthError += client_OnAuthError;
                client.OnLogin += client_OnLogin;
                client.OnMessage += client_OnMessage;
                client.Open();

                Log.Write("GTalk Tracking Started");
            }
            catch (Exception exception)
            {
                client = null;
                Log.Error(exception);
            }
        }

        public static void StopTracking()
        {
            if (client != null)
            {
                client.OnAuthError -= client_OnAuthError;
                client.OnLogin -= client_OnLogin;
                client.OnMessage -= client_OnMessage;
                client.Close();
                client = null;

                Log.Write("GTalk Tracking Stopped");
            }
        }

        #endregion

        #region Events

        static void client_OnAuthError(object sender, Element e)
        {
            Log.Write("GTalk Authentication Failed: " + Constants.GTALK_USERNAME);
        }

        static void client_OnLogin(object sender)
        {
            Log.Write("GTalk Login Success: " + Constants.GTALK_USERNAME);
        }

        static void client_OnMessage(object sender, Message msg)
        {
            if (!Constants.GTALK_ENABLED) return;

            try
            {
                if (client != null)
                {
                    string from = msg.From.ToString().Split('/')[0];
                    Jid jid = new Jid(from);

                    //Log.Write("GTalk: New chat from '" + from + "': " + msg.Body);

                    var autoReplyAction = new Action<string>(text =>
                    {
                        Message replyMessage = new Message(jid, MessageType.chat, text);
                        client.Send(replyMessage);
                    });
                    Execution.ExecuteBusyRecipe("GTalk", from, autoReplyAction);
                    Execution.ExecuteBatteryLowRecipe("GTalk", from, autoReplyAction);
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
