#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public interface ILiteral { }

    public class Numeric : ILiteral { }

    public class Character : ILiteral { }

    public class String : ILiteral { }

    public interface ILiteralToken
    {
        public string Value { get; }
    }

    public class LiteralToken<T> : Token, ILiteralToken where T : ILiteral
    {
        public string Value { get; }

        public LiteralToken(Point location, string value) : base(location) => Value = value;
    }
}
