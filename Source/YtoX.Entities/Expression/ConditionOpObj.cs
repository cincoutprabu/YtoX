//ConditionOpObj.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace YtoX.Entities.Expression
{
    public class ConditionOpObj
    {
        #region Properties

        public ConditionOp Symbol { get; set; }
        public string SymbolText { get; set; }
        public string Description { get; set; }
        public int RightExpressionCount { get; set; }

        #endregion

        #region Constructors

        public ConditionOpObj()
        {
            Symbol = ConditionOp.None;
            SymbolText = string.Empty;
            Description = string.Empty;
            RightExpressionCount = 0;
        }

        #endregion
    }

    public static class AllConditionOperators
    {
        public static List<ConditionOpObj> Operators { get; set; }

        #region Constructors

        static AllConditionOperators()
        {
            Operators = new List<ConditionOpObj>();

            Operators.Add(new ConditionOpObj()
            {
                Symbol = ConditionOp.Equal,
                SymbolText = "=",
                Description = "Equals",
                RightExpressionCount = 1
            });

            Operators.Add(new ConditionOpObj()
            {
                Symbol = ConditionOp.NotEqual,
                SymbolText = "!=",
                Description = "Not Equals",
                RightExpressionCount = 1
            });

            Operators.Add(new ConditionOpObj()
            {
                Symbol = ConditionOp.GreaterThan,
                SymbolText = ">",
                Description = "Greater Than",
                RightExpressionCount = 1
            });

            Operators.Add(new ConditionOpObj()
            {
                Symbol = ConditionOp.LessThan,
                SymbolText = "<",
                Description = "Less Than",
                RightExpressionCount = 1
            });

            Operators.Add(new ConditionOpObj()
            {
                Symbol = ConditionOp.GreaterThanOrEqual,
                SymbolText = ">=",
                Description = "Greater Than Or Equals",
                RightExpressionCount = 1
            });

            Operators.Add(new ConditionOpObj()
            {
                Symbol = ConditionOp.LessThanOrEqual,
                SymbolText = "<=",
                Description = "Less Than Or Equals",
                RightExpressionCount = 1
            });

            Operators.Add(new ConditionOpObj()
            {
                Symbol = ConditionOp.Between,
                SymbolText = "BETWEEN",
                Description = "Between",
                RightExpressionCount = 2
            });

            Operators.Add(new ConditionOpObj()
            {
                Symbol = ConditionOp.In,
                SymbolText = "IN",
                Description = "In",
                RightExpressionCount = Int32.MaxValue
            });

            Operators.Add(new ConditionOpObj()
            {
                Symbol = ConditionOp.Contains,
                SymbolText = "CONTAINS",
                Description = "Contains",
                RightExpressionCount = 1
            });
        }

        #endregion

        #region Methods

        public static ConditionOpObj Find(ConditionOp symbol)
        {
            return Operators.FirstOrDefault(op => op.Symbol == symbol);
        }

        #endregion
    }
}
