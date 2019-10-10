//DropboxHelper.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using YtoX.Entities;

namespace YtoX.Core.Helpers
{
    public class DropboxHelper
    {
        private static string DropboxPath = string.Empty;

        private static List<FileSystemWatcher> watcherList = new List<FileSystemWatcher>();

        #region Methods

        public static void LoadDropbox()
        {
            try
            {
                var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"Dropbox\host.db");

                string[] lines = File.ReadAllLines(dbPath);
                byte[] dbBase64Text = Convert.FromBase64String(lines[1]);
                DropboxPath = Encoding.ASCII.GetString(dbBase64Text);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void StartTracking()
        {
            if (string.IsNullOrEmpty(DropboxPath)) LoadDropbox();
            if (string.IsNullOrEmpty(DropboxPath)) return;

            try
            {
                string[] docTypes = Constants.DROPBOX_DOCUMENTS.Split(';');
                foreach (string type in docTypes)
                {
                    string[] filters = Constants.DocumentFilters[type];
                    foreach (string filter in filters)
                    {
                        FileSystemWatcher watcher = new FileSystemWatcher(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filter);
                        watcher.IncludeSubdirectories = false;
                        watcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.CreationTime | NotifyFilters.Attributes;
                        watcher.Created += documentsWatcher_Created;
                        watcher.Changed += documentsWatcher_Changed;
                        watcher.Deleted += documentsWatcher_Deleted;
                        watcher.Renamed += documentsWatcher_Renamed;
                        watcherList.Add(watcher);
                    }
                }

                watcherList.ForEach(obj => obj.EnableRaisingEvents = true);
                Log.Write("Dropbox Tracking Started");
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }
        }

        public static void StopTracking()
        {
            if (watcherList != null)
            {
                foreach (var watcher in watcherList)
                {
                    watcher.EnableRaisingEvents = false;
                    watcher.Created -= documentsWatcher_Created;
                    watcher.Changed -= documentsWatcher_Changed;
                    watcher.Deleted -= documentsWatcher_Deleted;
                    watcher.Renamed -= documentsWatcher_Renamed;
                }

                Log.Write("Dropbox Tracking Stopped");
            }
        }

        #endregion

        #region Events

        static void documentsWatcher_Created(object sender, FileSystemEventArgs e)
        {
            Execution.ExecuteDocumentsRecipe(e.FullPath, Path.Combine(DropboxPath, e.Name), false);
        }

        static void documentsWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            Execution.ExecuteDocumentsRecipe(e.FullPath, Path.Combine(DropboxPath, e.Name), true);
        }

        static void documentsWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            //
        }

        static void documentsWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            Execution.ExecuteDocumentsRecipe(e.FullPath, Path.Combine(DropboxPath, e.Name), false);
        }

        #endregion
    }
}
