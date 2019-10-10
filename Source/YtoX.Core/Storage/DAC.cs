//DAC.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;

namespace YtoX.Core.Storage
{
    public class DAC : IDisposable
    {
        public static string DatabasePath = string.Empty;
        private SQLiteConnection connection = null;

        #region Constructors

        public DAC()
        {
            try
            {
                SetupDB();
                connection = new SQLiteConnection("Data Source=" + DatabasePath + ";Version=3;");
                connection.Open();

                //SetupTables();
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        #endregion

        #region Methods

        public DbDataReader FetchRecords(string query)
        {
            SQLiteCommand command = new SQLiteCommand(query, connection);
            DbDataReader reader = command.ExecuteReader();
            return reader;
        }

        public int ExecuteCommand(string statement)
        {
            SQLiteCommand command = new SQLiteCommand(statement, connection);
            return command.ExecuteNonQuery();
        }

        public int ExecuteCommand(SQLiteCommand command)
        {
            command.Connection = connection;
            return command.ExecuteNonQuery();
        }

        public object ExecuteScalar(string statement)
        {
            SQLiteCommand command = new SQLiteCommand(statement, connection);
            return command.ExecuteScalar();
        }

        #endregion

        #region Internal-Methods

        public static bool SetupDB()
        {
            DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "YtoX.sqlite");
            if (File.Exists(DatabasePath)) return true;

            DatabasePath = @"C:\PRABU\Dropbox\codeding\YtoX\YtoX.sqlite";
            if (File.Exists(DatabasePath)) return true;

            return false;
        }

        [Obsolete]
        private void SetupTables()
        {
            if (!TableExists("Settings"))
            {
                string[] columns = { "SettingKey TEXT", "SettingValue TEXT" };
                CreateTable("Settings", columns);
            }
        }

        private void CreateTable(string tableName, string[] columns)
        {
            string statement = string.Format("CREATE TABLE {0} ({1});", tableName, string.Join(", ", columns));
            ExecuteCommand(statement);
        }

        private bool TableExists(string tableName)
        {
            string query = string.Format("SELECT name FROM sqlite_master WHERE type = 'table' AND name = '{0}';", tableName);
            using (DbDataReader reader = FetchRecords(query))
            {
                return reader.HasRows;
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if (connection != null)
            {
                //if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        #endregion
    }
}
