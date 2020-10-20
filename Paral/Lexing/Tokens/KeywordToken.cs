using System.Drawing;

namespace Paral.Lexing.Tokens
{
    public enum Keyword {
        Requires,
        Namespace,
        Implements,
        Declares,
        Throws,
        Struct,
        Returns,
        Return
    }

    public class KeywordToken : Token
    {
        public Keyword Value { get; }

        public KeywordToken(Point location, Keyword keyword) : base(location) => Value = keyword;
    }
}
