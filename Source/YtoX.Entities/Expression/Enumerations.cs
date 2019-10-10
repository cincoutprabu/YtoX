//Enumerations.cs

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace YtoX.Entities.Expression
{
    public enum DataType
    {
        None = 0,
        Text = 1,
        Number = 2,
        Date = 3,
        Time = 4, //eg. today, tomorrow
        TimeSpan = 5,
        Boolean = 6,
        Weather = 7,
        Location = 8,
        DeviceOrientation = 9
    }

    //Type of a token in an expression
    public enum TokenType
    {
        None = 0,
        DataAttribute = 1,
        Routine = 2,
        Constant = 3,
        Operator = 5,
        Unassigned = 6,
        DummyRoutineEnd = 7, //represents ')'
        DummyArgumentSeparator = 8 //represents ','
    }

    ///Type of a routine/function
    public enum RoutineType
    {
        None = 0,
        Text = 1,
        Number = 2,
        DateTime = 3,
        Statistical = 4
    }

    //Operators used within an expression
    public enum ExpressionOp
    {
        None = 0,
        Plus = 1,
        Minus = 2,
        Multiply = 3,
        Divide = 4,
        Modulus = 5,
        OpenParenthesis = 6,
        CloseParenthesis = 7
    }

    //List of possible condition/comparison operators
    public enum ConditionOp
    {
        None = 0,
        Equal = 1,
        NotEqual = 2,
        GreaterThan = 3,
        LessThan = 4,
        GreaterThanOrEqual = 5,
        LessThanOrEqual = 6,
        Between = 7,
        In = 8,
        Contains = 9
    }

    //Operators that combine conditions
    public enum LogicalOp
    {
        None = 0,
        AND = 1,
        OR = 2
    }
}
