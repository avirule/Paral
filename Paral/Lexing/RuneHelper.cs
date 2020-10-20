#region

using System.Text;

#endregion


namespace Paral.Lexing
{
    public static class RuneHelper
    {
        public static class Operators
        {
            public static Rune Add { get; } = (Rune)'+';
            public static Rune Subtract { get; } = (Rune)'-';
            public static Rune Multiply { get; } = (Rune)'*';
            public static Rune Divide { get; } = (Rune)'/';
        }

        public static class Blocks
        {
            public static Rune ParenthesisOpen { get; } = (Rune)'(';
            public static Rune ParenthesisClose { get; } = (Rune)')';
            public static Rune BracketOpen { get; } = (Rune)'{';
            public static Rune BracketClose { get; } = (Rune)'}';
            public static Rune SquareBracketOpen { get; } = (Rune)'[';
            public static Rune SquareBracketClose { get; } = (Rune)']';
        }

        public static Rune CarriageReturn { get; } = (Rune)'\r';
        public static Rune NewLine { get; } = (Rune)'\n';
        public static Rune Colon { get; } = (Rune)':';
        public static Rune Semicolon { get; } = (Rune)';';
        public static Rune Comma { get; } = (Rune)',';
        public static Rune Period { get; } = (Rune)'.';
    }
}
