using System;
using System.Drawing;
using System.Text;

namespace Paral.Compiler
{
    public class InvalidTokenException : Exception
    {
        public Point Location { get; }
        public Rune Character { get; }

        public InvalidTokenException(Point location, Rune character) : base(FormatHelper.InvalidToken(location, character)) =>
            (Location, Character) = (location, character);
    }
}
