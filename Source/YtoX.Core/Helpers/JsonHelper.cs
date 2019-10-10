//JsonHelper.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Reflection;

using MongoDB.Bson;
using MongoDB.Bson.IO;

using YtoX.Entities;

namespace YtoX.Core.Helpers
{
    public class JsonHelper
    {
        #region Methods

        public static Dictionary<string, object> ObjectToDictionary(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();

            Dictionary<string, object> dict = new Dictionary<string, object>();
            foreach (PropertyInfo property in properties)
            {
                dict.Add(property.Name, property.GetValue(obj, null));
            }

            return (dict);
        }

        public static string Serialize(object obj)
        {
            try
            {
                JsonWriterSettings settings = new JsonWriterSettings();
                settings.Indent = false;

                Dictionary<string, object> dict = ObjectToDictionary(obj);
                BsonDocument document = new BsonDocument(dict);
                return document.ToJson(settings);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static BsonDocument Deserialize(string json)
        {
            try
            {
                return BsonDocument.Parse(json);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [Obsolete("Not in Use", true)]
        public static Dictionary<string, object> DeserializeAsDictionary(string json)
        {
            try
            {
                BsonDocument document = BsonDocument.Parse(json);
                return document.ToDictionary();
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion
    }
}
