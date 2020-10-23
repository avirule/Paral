#region

using System.Drawing;
using System.Linq;
using System.Text;

#endregion


namespace Paral.Lexing.Tokens
{
    public class NumericLiteralToken : Token
    {
        public bool IsDecimal { get; }
        public string Value { get; }

        public NumericLiteralToken(Point location, string value) : base(location)
        {
            foreach (Rune rune in value.EnumerateRunes().Where(rune => rune == (Rune)'.'))
            {
                if (IsDecimal) ThrowHelper.Throw(location, "Attempted to parse decimal literal with too many separators.");

                IsDecimal = true;
            }

            Value = value;
        }
    }
}
