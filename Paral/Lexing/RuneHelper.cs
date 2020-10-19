#region

using System.Text;

#endregion


namespace Paral.Lexing
{
    public static class RuneHelper
    {
        public static class Operators
        {
            public static Rune Add { get; } = new Rune('+');
            public static Rune Subtract { get; } = new Rune('-');
            public static Rune Multiply { get; } = new Rune('*');
            public static Rune Divide { get; } = new Rune('/');
        }

        public static Rune CarriageReturn { get; } = new Rune('\r');
        public static Rune NewLine { get; } = new Rune('\n');
        public static Rune Colon { get; } = new Rune(':');
        public static Rune Semicolon { get; } = new Rune(';');
    }
}
