//Routine.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace YtoX.Entities.Expression
{
    public class Routine : IToken
    {
        #region Properties

        public int UID { get; set; }
        public RoutineType RoutineType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsToken { get; set; }
        public DataType ReturnDataType { get; set; }
        public List<RoutineArg> Arguments { get; set; }
        public string Example { get; set; }

        #endregion

        #region Constructors

        public Routine()
        {
            UID = -1;
            RoutineType = RoutineType.None;
            Name = string.Empty;
            Description = string.Empty;
            IsToken = false;
            ReturnDataType = DataType.None;
            Arguments = new List<RoutineArg>();
            Example = string.Empty;
        }

        #endregion

        #region IExpressionToken Members

        public string ToExpression()
        {
            string expr = this.Name + "(";
            expr += string.Join(", ", (from argument in this.Arguments select argument.Name).ToArray());
            expr += ")";

            return (expr);
        }

        #endregion
    }
}
