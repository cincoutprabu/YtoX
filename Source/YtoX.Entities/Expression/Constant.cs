//Constant.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Globalization;

namespace YtoX.Entities.Expression
{
    public class Constant : IToken
    {
        public const string StringDelimiter = "'";

        #region Properties

        public DataType DataType { get; set; }
        public string Value { get; set; }

        #endregion

        #region Constructors

        public Constant()
        {
            DataType = DataType.None;
            Value = string.Empty;
        }

        #endregion

        #region Methods

        public Token BuildExpressionToken()
        {
            Token expressionToken = new Token();
            expressionToken.Type = TokenType.Constant;
            expressionToken.TokenObject = this;
            return expressionToken;
        }

        #endregion

        #region IExpressionToken Members

        public string ToExpression()
        {
            string expr = this.Value;
            if (DataType == DataType.Text || DataType == DataType.Date || DataType == DataType.Boolean)
            {
                expr = expr.Replace(StringDelimiter, string.Empty);
                expr = StringDelimiter + expr + StringDelimiter;
            }
            return expr;
        }

        #endregion

        #region Helper-Methods

        public static DataType ResolveDataType(string value)
        {
            //check if 'Date'
            DateTime dateTimeValue;
            if (DateTime.TryParseExact(value.Replace(StringDelimiter, string.Empty), "yyyy-MM-dd", null, DateTimeStyles.None, out dateTimeValue))
            {
                return (DataType.Date);
            }

            //check if 'Boolean'
            bool boolValue;
            if (Boolean.TryParse(value.Replace(StringDelimiter, string.Empty), out boolValue))
            {
                return (DataType.Boolean);
            }

            //check if 'Number'
            decimal numberValue;
            if (Decimal.TryParse(value, out numberValue))
            {
                return (DataType.Number);
            }

            return DataType.Text;
        }

        public static Constant Parse(string value)
        {
            Constant constant = new Constant();
            constant.DataType = ResolveDataType(value);
            constant.Value = value;
            return constant;
        }

        #endregion
    }


    public static class KnownConstants
    {
        public static List<Constant> Constants { get; set; }

        #region Constructors

        static KnownConstants()
        {
            Constants = new List<Constant>();

            Constants.Add(new Constant() { DataType = DataType.Time, Value = "TODAY" });
            Constants.Add(new Constant() { DataType = DataType.Time, Value = "TOMORROW" });
            Constants.Add(new Constant() { DataType = DataType.Time, Value = "YESTERDAY" });
            Constants.Add(new Constant() { DataType = DataType.Weather, Value = "SUNNY" });
            Constants.Add(new Constant() { DataType = DataType.Weather, Value = "RAINY" });
            Constants.Add(new Constant() { DataType = DataType.Weather, Value = "SNOWY" });
            Constants.Add(new Constant() { DataType = DataType.Weather, Value = "STORM" });
            Constants.Add(new Constant() { DataType = DataType.Weather, Value = "CYCLONE" });
        }

        #endregion
    }
}
