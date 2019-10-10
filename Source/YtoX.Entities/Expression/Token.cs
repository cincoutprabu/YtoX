//Token.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace YtoX.Entities.Expression
{
    public class Token
    {
        #region Properties

        public TokenType Type { get; set; }
        public IToken TokenObject { get; set; }

        #endregion

        #region Constructors

        public Token()
        {
            Type = TokenType.None;
            TokenObject = null;
        }

        public Token(TokenType tokenType, IToken tokenObject)
        {
            this.Type = tokenType;
            this.TokenObject = tokenObject;
        }

        #endregion

        #region Methods

        public string GetLinearToken()
        {
            string expr = string.Empty;

            //token-object will be null for 'Unassigned' and dummy-token-types
            if (TokenObject == null)
            {
                if (Type == TokenType.Unassigned)
                {
                    expr = string.Empty;
                }
                else if (Type == TokenType.DummyArgumentSeparator)
                {
                    expr = ",";
                }
                else if (Type == TokenType.DummyRoutineEnd)
                {
                    expr = ")";
                }
            }
            else
            {
                if (Type == TokenType.Routine)
                {
                    expr = ((Routine)TokenObject).Name + "(";
                }
                else
                {
                    expr = TokenObject.ToExpression();
                }
            }

            return (expr);
        }

        public RawToken BuildRawToken()
        {
            int tokenObjectUid = -1;
            //if (TokenObject is Entity)
            //{
            //    tokenObjectUid = ((Entity)TokenObject).UID;
            //}

            return (new RawToken(this.Type, tokenObjectUid, GetLinearToken()));
        }

        #endregion
    }

    public class ExpressionTokenList : List<Token> { }
}
