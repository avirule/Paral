using System.Drawing;

namespace Paral.Compiler.Lexing.Tokens
{
    public enum Operator
    {
        Add,
        Subtract,
        Multiply,
        Divide,
        Assign,
        Compare,
        TypeCompare
    }

    public class OperatorToken : Token
    {
        public Operator Operator { get; }

        public OperatorToken(Point location, Operator @operator) : base(location) => Operator = @operator;
    }
}
