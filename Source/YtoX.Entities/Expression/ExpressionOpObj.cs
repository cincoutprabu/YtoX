//ExpressionOpObj.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace YtoX.Entities.Expression
{
    public class ExpressionOpObj : IToken
    {
        #region Properties

        public ExpressionOp Symbol { get; set; }
        public string SymbolText { get; set; }
        public int OperandCount { get; set; }

        /// <summary>
        /// Indicates precedence over other operators in the expression. Operators with higher precedence
        /// are evaluated before operators with lower precedence. The precedence-level value starts with 1 (highest).
        /// (i.e. the lesser the number, the higher the precedence).
        /// </summary>
        public int PrecedenceLevel { get; set; }
        public List<DataType> CompatibleDataTypes { get; set; }

        #endregion

        #region Constructors

        public ExpressionOpObj()
        {
            Symbol = ExpressionOp.None;
            SymbolText = string.Empty;
            OperandCount = 0;
            PrecedenceLevel = -1;
            CompatibleDataTypes = new List<DataType>();
        }

        #endregion

        #region IExpressionToken Members

        public string ToExpression()
        {
            return SymbolText;
        }

        #endregion
    }

    public static class AllOperators
    {
        public static List<ExpressionOpObj> Operators { get; set; }

        #region Constructors

        static AllOperators()
        {
            Operators = new List<ExpressionOpObj>();

            Operators.Add(new ExpressionOpObj()
            {
                Symbol = ExpressionOp.Plus,
                SymbolText = "+",
                OperandCount = 2,
                PrecedenceLevel = 4,
                CompatibleDataTypes = (new DataType[] { DataType.Number, DataType.Text }).ToList()
            });

            Operators.Add(new ExpressionOpObj()
            {
                Symbol = ExpressionOp.Minus,
                SymbolText = "-",
                OperandCount = 2,
                PrecedenceLevel = 4,
                CompatibleDataTypes = (new DataType[] { DataType.Number }).ToList()
            });

            Operators.Add(new ExpressionOpObj()
            {
                Symbol = ExpressionOp.Multiply,
                SymbolText = "*",
                OperandCount = 2,
                PrecedenceLevel = 3,
                CompatibleDataTypes = (new DataType[] { DataType.Number }).ToList()
            });

            Operators.Add(new ExpressionOpObj()
            {
                Symbol = ExpressionOp.Divide,
                SymbolText = "/",
                OperandCount = 2,
                PrecedenceLevel = 3,
                CompatibleDataTypes = (new DataType[] { DataType.Number }).ToList()
            });

            Operators.Add(new ExpressionOpObj()
            {
                Symbol = ExpressionOp.Modulus,
                SymbolText = "%",
                OperandCount = 2,
                PrecedenceLevel = 3,
                CompatibleDataTypes = (new DataType[] { DataType.Number }).ToList()
            });

            Operators.Add(new ExpressionOpObj()
            {
                Symbol = ExpressionOp.OpenParenthesis,
                SymbolText = "(",
                OperandCount = 0,
                PrecedenceLevel = 1,
            });

            Operators.Add(new ExpressionOpObj()
            {
                Symbol = ExpressionOp.CloseParenthesis,
                SymbolText = ")",
                OperandCount = 0,
                PrecedenceLevel = 1,
            });
        }

        #endregion

        #region Methods

        public static ExpressionOpObj Find(ExpressionOp symbol)
        {
            return Operators.FirstOrDefault(op => op.Symbol == symbol);
        }

        public static ExpressionOpObj Find(string symbolText)
        {
            return Operators.FirstOrDefault(op => op.SymbolText.Equals(symbolText));
        }

        #endregion
    }
}
