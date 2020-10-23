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

    public class ArithmeticToken : Token
    {
        public ArithmeticOperator Operator { get; }
        public ArithmeticToken(Point location, ArithmeticOperator @operator) : base(location) => Operator = @operator;
    }
}
