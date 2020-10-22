#region

using System.Drawing;
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
            foreach (Rune rune in value.EnumerateRunes())
            {
                if (!rune.Equals(RuneHelper.Period)) continue;

                if (IsDecimal) ThrowHelper.Throw(location, "Attempted to parse decimal literal with invalid separator count.");

                IsDecimal = true;
            }

            Value = value;
        }
    }
}
