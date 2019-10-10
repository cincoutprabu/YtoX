//XmlHelper.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace YtoX.Entities
{
    public class XmlHelper
    {
        #region Methods

        public static string GetAttributeAsString(XmlNode node, string attributeName)
        {
            return node.Attributes[attributeName] != null ? node.Attributes[attributeName].Value : string.Empty;
        }

        public static bool GetAttributeAsBoolean(XmlNode node, string attributeName)
        {
            return node.Attributes[attributeName] != null ? Convert.ToBoolean(node.Attributes[attributeName].Value) : false;
        }

        public static XmlAttribute CreateAttribute(XmlDocument document, string name, string value)
        {
            XmlAttribute attribute = document.CreateAttribute(name);
            attribute.Value = value;
            return attribute;
        }

        #endregion
    }
}
