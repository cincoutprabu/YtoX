//TwitterHelper.cs

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Diagnostics;

using Twitterizer;

namespace YtoX.Core.Helpers
{
    public class TwitterHelper
    {
        private static string ConsumerKey = "MfdDAJZmbICwEQJJ6WgA";
        private static string ConsumerSecret = "AXjmc9pg4zU45kfDxa9qMofo7LFCsz1TzROchBwHmY";
        //private static string AccessToken = "63904128-eOOwBraOJ569aAcxYnpTHUCDmzANsqzpprouf7378";
        //private static string AccessTokenSecret = "Of8KGM3EBXDAj1hrZo5FVYQCrljT2fuNRS1yJ7bc1g";

        private static string SampleTweet = "Hello from YtoX!";

        #region Properties

        public static void PostTweet()
        {
            try
            {
                //get approval from user
                OAuthTokenResponse requestToken = OAuthUtility.GetRequestToken(ConsumerKey, ConsumerSecret, "oob");
                string url = String.Format("http://twitter.com/oauth/authorize?oauth_token={0}", requestToken.Token);
                Process.Start(url);

                //get PIN & access-token from user
                string pin = "1234";
                OAuthTokenResponse accessToken = OAuthUtility.GetAccessToken(ConsumerKey, ConsumerKey, requestToken.Token, pin);

                //post tweet
                OAuthTokens tokens = new OAuthTokens();
                tokens.AccessToken = accessToken.Token;
                tokens.AccessTokenSecret = accessToken.TokenSecret;
                tokens.ConsumerKey = ConsumerKey;
                tokens.ConsumerSecret = ConsumerSecret;

                TwitterResponse<TwitterStatus> tweetResponse = TwitterStatus.Update(tokens, SampleTweet);
                if (tweetResponse.Result == RequestResult.Success)
                {
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
