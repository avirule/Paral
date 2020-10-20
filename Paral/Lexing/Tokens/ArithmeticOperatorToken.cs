#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public enum ArithmeticOperator
    {
        Add,
        Subtract,
        Multiply,
        Divide
    }

    public class ArithmeticOperatorToken : Token
    {
        public ArithmeticOperator Value { get; }

        public ArithmeticOperatorToken(Point location, ArithmeticOperator value) : base(location) => Value = value;
    }
}
