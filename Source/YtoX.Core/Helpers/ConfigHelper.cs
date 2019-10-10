//ConfigHelper.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

using YtoX.Entities;
using YtoX.Core;

namespace YtoX.Core.Helpers
{
    public class ConfigHelper
    {
        #region Methods

        public static Dictionary<string, string> ReadAppSettings(string configFilePath)
        {
            Dictionary<string, string> appSettings = new Dictionary<string, string>();

            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(configFilePath);

                XmlNodeList settingsNode = document.DocumentElement.GetElementsByTagName("appSettings");
                if (settingsNode.Count > 0)
                {
                    foreach (XmlNode node in settingsNode[0].ChildNodes)
                    {
                        appSettings.Add(node.Attributes["key"].Value, node.Attributes["value"].Value);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error(exception);
            }

            return appSettings;
        }

        #endregion
    }
}
