//Expression.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace YtoX.Entities.Expression
{
    public class Expression
    {
        #region Properties

        public List<RawToken> Tokens { get; set; }
        public DataType ResultDataType { get; set; }

        public string LinearExpression
        {
            get
            {
                return (string.Concat((from rawToken in this.Tokens select rawToken.LinearToken).ToArray()));
            }
        }

        #endregion

        #region Constructors

        public Expression()
        {
            Tokens = new List<RawToken>();
            ResultDataType = DataType.None;
        }

        public Expression(List<RawToken> tokens)
        {
            this.Tokens = tokens;
            this.ResultDataType = DataType.None;
        }

        public Expression(RawToken token)
        {
            this.Tokens = new List<RawToken>();
            this.Tokens.Add(token);
            this.ResultDataType = DataType.None;
        }

        #endregion

        #region Methods

        public void ResetTokens(List<RawToken> tokens, DataType resultDataType)
        {
            this.Tokens.Clear();
            this.Tokens.AddRange(tokens);
            this.ResultDataType = resultDataType;
        }

        #endregion
    }
}
