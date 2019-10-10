//NewsDAC.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

using YtoX.Entities;
using YtoX.Core.News;

namespace YtoX.Core.Storage
{
    public class NewsDAC
    {
        #region Methods

        public static List<NewsEntry> FetchAll()
        {
            List<NewsEntry> all = new List<NewsEntry>();
            using (DAC dac = new DAC())
            {
                using (DbDataReader reader = dac.FetchRecords("SELECT * FROM News;"))
                {
                    while (reader.Read())
                    {
                        all.Add(Fetch(reader));
                    }
                }
            }
            return all;
        }

        public static void Clear()
        {
            using (DAC dac = new DAC())
            {
                string statement = string.Format("DELETE FROM News;");
                dac.ExecuteCommand(statement);
            }
        }

        public static void Insert(NewsEntry entry)
        {
            entry.Title = entry.Title.Replace("'", "''");
            entry.Summary = entry.Summary.Replace("'", "''");

            using (DAC dac = new DAC())
            {
                string statement = string.Format("INSERT INTO News (Provider, Title, Url, EmailId, EmailSent) VALUES ('{0}', '{1}', '{2}', '{3}', {4});", entry.Provider, entry.Title, entry.Url, entry.EmailId, entry.EmailSent ? 1 : 0);
                dac.ExecuteCommand(statement);
            }
        }

        public static bool IsDuplicate(NewsEntry entry)
        {
            using (DAC dac = new DAC())
            {
                string query = string.Format("SELECT Title FROM News WHERE Provider = '{0}' AND Title = '{1}' AND EmailId = '{2}';", entry.Provider, entry.Title, entry.EmailId);
                using (DbDataReader reader = dac.FetchRecords(query))
                {
                    return reader.HasRows;
                }
            }
        }

        #endregion

        #region Internal-Methods

        private static NewsEntry Fetch(DbDataReader reader)
        {
            NewsEntry entry = new NewsEntry();
            try
            {
                entry.Provider = (string)reader["Provider"];
                entry.Title = (string)reader["Title"];
                if (!Convert.IsDBNull(reader["Summary"])) entry.Summary = (string)reader["Summary"];
                entry.Url = (string)reader["Url"];
                entry.EmailId = (string)reader["EmailId"];
                entry.EmailSent = (bool)reader["EmailSent"];
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
            return entry;
        }

        #endregion
    }
}
