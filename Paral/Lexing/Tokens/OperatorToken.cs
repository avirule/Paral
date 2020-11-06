using System.Drawing;

namespace Paral.Lexing.Tokens
{
    public enum Operator
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Assign,
        Compare
    }

    public class OperatorToken : Token
    {
        public Operator Operator { get; }

        public OperatorToken(Point location, Operator @operator) : base(location) => Operator = @operator;
    }
}
