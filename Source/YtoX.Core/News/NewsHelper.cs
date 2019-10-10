//NewsHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ServiceModel.Syndication;

using YtoX.Entities;
using YtoX.Core.Helpers;
using YtoX.Core.Storage;

namespace YtoX.Core.News
{
    public class NewsHelper
    {
        public static DateTime LastReadOn = default(DateTime);
        public static List<NewsEntry> NewsList = new List<NewsEntry>();

        #region Methods

        public static bool ReadNews()
        {
            if (string.IsNullOrEmpty(Constants.NEWS_EMAIL))
            {
                return false;
            }

            //ignore if news is read very recently
            if (LastReadOn != default(DateTime) && DateTime.Now.Subtract(LastReadOn).TotalMinutes < Constants.NEWS_READ_INTERVAL)
            {
                return false;
            }

            try
            {
                NewsList.Clear();

                var selectedProviders = Constants.NEWS_PROVIDERS.Split(';');
                foreach (string provider in selectedProviders)
                {
                    ProcessRss(provider, Constants.AllNewsProviders[provider]);
                }

                LastReadOn = DateTime.Now;
                return true;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return false;
        }

        private static void ProcessRss(string key, string rssUrl)
        {
            try
            {
                //Log.Write("Fetching News from: " + key);

                XmlReaderSettings settings = new XmlReaderSettings() { MaxCharactersInDocument = Int32.MaxValue };
                XmlReader reader = XmlReader.Create(rssUrl, settings);

                SyndicationFeed feed = SyndicationFeed.Load(reader);
                foreach (var item in feed.Items)
                {
                    var entry = new NewsEntry()
                    {
                        Provider = key,
                        Title = item.Title.Text,
                        Summary = item.Summary.Text,
                        Url = item.Links.Count > 0 ? item.Links[0].Uri.AbsoluteUri : string.Empty,
                        EmailId = Constants.NEWS_EMAIL,
                        EmailSent = false
                    };
                    NewsList.Add(entry);

                    //Log.Write("News Article: " + entry.Title);
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void GetMatchingAndSendEmail(string[] keywords)
        {
            for (int index = 0; index < NewsList.Count; index++)
            {
                NewsEntry entry = NewsList[index];
                if (ContainsAny(entry.Title, keywords) || ContainsAny(entry.Summary, keywords))
                {
                    if (!NewsDAC.IsDuplicate(entry))
                    {
                        SendNewsAlertEmail(ref entry, keywords);
                        if (entry.EmailSent)
                        {
                            NewsDAC.Insert(entry);
                        }
                    }
                }
            }
        }

        public static bool ContainsAny(string text, string[] keywords)
        {
            foreach (string keyword in keywords)
            {
                if (text.ToLower().Contains(keyword.Trim().ToLower())) return true;
            }

            return false;
        }

        public static void SendNewsAlertEmail(ref NewsEntry entry, string[] keywords)
        {
            try
            {
                string warningText = "You are receiving this mail, because you have enabled News-based-recipes in YtoX.";

                string body = @"You might be interested in this:";
                body += "<p><a href=\"" + entry.Url + "\" target=\"_blank\">" + entry.Title + "</a></p>";
                body += "<p>Provider: <a href=\"" + Constants.AllNewsProviders[entry.Provider] + "\" target=\"_blank\">" + entry.Provider + " RSS</a></p>";
                body += "<p>Relevant Keywords: " + string.Join(", ", keywords) + "</p>";
                body += "<pre>" + warningText + "</pre>";

                string subject = "YtoX News Alert";
                EmailHelper.SendEmail(Constants.NEWS_EMAIL, subject, body);

                entry.EmailSent = true;
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion
    }
}
