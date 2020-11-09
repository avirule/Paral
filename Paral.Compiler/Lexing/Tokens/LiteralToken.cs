#region

using System.Drawing;

#endregion


namespace Paral.Compiler.Lexing.Tokens
{
    public enum Literal
    {
        Numeric,
        String,
        Character
    }

    public class LiteralToken : Token
    {
        public Literal Type { get; }
        public string Value { get; }

        public LiteralToken(Point location, Literal type, string value) : base(location) => (Type, Value) = (type, value);
    }
}
