#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public interface ILiteral { }

    public class String : ILiteral { }

    public class Character : ILiteral { }

    public class Numeric : ILiteral { }

    public class LiteralToken<T> : Token where T : ILiteral
    {
        public string Value { get; }

        public LiteralToken(Point location, string value) : base(location) => Value = value;
    }
}
