//Condition.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace YtoX.Entities.Expression
{
    public class Condition
    {
        #region Properties

        public int Id { get; set; }
        public string Description { get; set; }
        public Expression Left { get; set; }
        public ConditionOp Operator { get; set; }
        public List<Expression> Right { get; set; }
        public int Ordinal { get; set; }

        #endregion

        #region Constructors

        public Condition()
        {
            Id = -1;
            Description = string.Empty;
            Left = new Expression();
            Operator = ConditionOp.None;
            Right = new List<Expression>();
            Ordinal = -1;
        }

        #endregion

        #region Methods

        public string ToLinearExpression()
        {
            ConditionOpObj op = AllConditionOperators.Find(this.Operator);

            string expression = string.Empty;
            switch (this.Operator)
            {
                case ConditionOp.Equal:
                case ConditionOp.NotEqual:
                case ConditionOp.GreaterThan:
                case ConditionOp.LessThan:
                case ConditionOp.GreaterThanOrEqual:
                case ConditionOp.LessThanOrEqual:
                    {
                        expression = Left.LinearExpression + " " + op.SymbolText + " ";
                        expression += Right.First().LinearExpression;
                        break;
                    }
                case ConditionOp.Between:
                    {
                        expression = Left.LinearExpression + " " + op.SymbolText + " ";
                        expression += Right[0].LinearExpression + " AND " + Right[1].LinearExpression;
                        break;
                    }
                case ConditionOp.In:
                    {
                        expression = Left.LinearExpression + " " + op.SymbolText + " (";
                        foreach (Expression ex in Right)
                        {
                            expression += ex.LinearExpression;
                            if (Right.IndexOf(ex) < Right.Count - 1)
                            {
                                expression += ", ";
                            }
                        }
                        expression += ")";
                        break;
                    }
            }

            return (expression);
        }

        #endregion
    }
}
