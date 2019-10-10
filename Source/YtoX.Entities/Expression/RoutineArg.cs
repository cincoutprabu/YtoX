//RoutineArg.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace YtoX.Entities.Expression
{
    public class RoutineArg
    {
        #region Properties

        public string Name { get; set; }
        public string Description { get; set; }
        public DataType DataType { get; set; }
        public bool Optional { get; set; }

        #endregion

        #region Constructors

        public RoutineArg()
        {
            Name = string.Empty;
            Description = string.Empty;
            DataType = DataType.None;
            Optional = false;
        }

        #endregion
    }
}
