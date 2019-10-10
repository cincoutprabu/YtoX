//WebFormHelper.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;

namespace YtoX.Core.Helpers
{
    public class WebHelper
    {
        #region Methods

        public static string ReadUrl(string url)
        {
            WebClient client = new WebClient();
            string result = client.DownloadString(url);
            return result;
        }

        public static string ReadService(string serviceUrl, string arguments)
        {
            try
            {
                //create web-request
                WebRequest request = (WebRequest)WebRequest.Create(serviceUrl);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Proxy = HttpWebRequest.GetSystemWebProxy();
                request.Proxy.Credentials = CredentialCache.DefaultCredentials;

                //send request
                byte[] argumentBytes = Encoding.ASCII.GetBytes(arguments);
                request.ContentLength = argumentBytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(argumentBytes, 0, argumentBytes.Length);
                requestStream.Close();

                //read response
                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                return reader.ReadToEnd();
            }
            catch (Exception)
            { }

            return string.Empty;
        }

        #endregion
    }
}
