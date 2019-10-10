//FacebookHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Facebook;

namespace YtoX.Core.Helpers
{
    public class FacebookHelper
    {
        private static string AppId = "410103425729696"; //YtoX AppId
        private static string AppSecret = "14f2c7b041991bc23bcbcb9b6ddee513";
        private static FacebookClient client = null;

        #region Methods

        public static void StartTracking()
        {
            try
            {
                client = new FacebookClient(AppId, AppSecret);
                client.GetCompleted += client_GetCompleted;
                client.PostCompleted += client_PostCompleted;
                client.UploadProgressChanged += client_UploadProgressChanged;

                //do login
                client.Get(BuildLoginParams());
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void StopTracking()
        {
            if (client != null)
            {
                client.GetCompleted -= client_GetCompleted;
                client.PostCompleted -= client_PostCompleted;
                client.UploadProgressChanged -= client_UploadProgressChanged;
                client = null;
            }
        }

        private static Dictionary<string, object> BuildLoginParams()
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("client_id", AppId);
            parameters.Add("redirect_uri", "https://www.facebook.com/connect/login_success.html");
            parameters.Add("response_type", "token");
            parameters.Add("display", "popup");
            return parameters;
        }

        #endregion

        #region Events

        static void client_GetCompleted(object sender, FacebookApiEventArgs e)
        {
            //
        }

        static void client_PostCompleted(object sender, FacebookApiEventArgs e)
        {
            //
        }

        static void client_UploadProgressChanged(object sender, FacebookUploadProgressChangedEventArgs e)
        {
            //
        }

        #endregion
    }
}
