using System.Drawing;

namespace Paral.Lexing.Tokens
{
    public enum SeparatorType
    {
        Comma
    }

    public class SeparatorToken : Token
    {
        public SeparatorType Type { get; }

        public SeparatorToken(Point location, SeparatorType type) : base(location) { Type = type; }
    }
}
