using System.Drawing;

namespace Paral.Lexing.Tokens
{
    public class IdentifierToken : Token
    {
        public string Value { get; }

        public IdentifierToken(Point location, string value) : base(location) { Value = value; }
    }
}
