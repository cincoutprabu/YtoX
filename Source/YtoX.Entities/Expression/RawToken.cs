//RawToken.cs

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Runtime.Serialization;

namespace YtoX.Entities.Expression
{
    public class RawToken
    {
        #region Properties

        public TokenType Type { get; set; }
        public int UID { get; set; }
        public string LinearToken { get; set; }
        public string MetaData { get; set; }
        public int Index { get; set; }

        #endregion

        #region Constructors

        public RawToken()
        {
            Type = TokenType.None;
            UID = -1;
            LinearToken = string.Empty;
            MetaData = string.Empty;
            Index = -1;
        }

        public RawToken(TokenType type, int uid, string linearToken)
        {
            this.Type = type;
            this.UID = uid;
            this.LinearToken = linearToken;
            this.MetaData = string.Empty;
            this.Index = -1;
        }

        #endregion

        #region Methods

        public bool IsEquals(object obj)
        {
            RawToken rawToken = (RawToken)obj;
            return (this.Type == rawToken.Type && this.UID == rawToken.UID && this.LinearToken.Equals(rawToken.LinearToken));
        }

        public override string ToString()
        {
            return ("{" + Type + "," + UID + "," + LinearToken + "}");
        }

        #endregion
    }
}
