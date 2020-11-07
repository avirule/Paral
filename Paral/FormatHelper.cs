using System;
using System.Drawing;
using System.Text;
using Paral.Lexing.Tokens;

namespace Paral
{
    public static class FormatHelper
    {
        private const string _LEXER_ERROR_TEMPLATE = "[Col: {0}, Row: {1}] {2}";
        private const string _EXPECTED_TOKEN_FORMAT = "Expected token {0} (actual {1}).";

        public static string ExpectedToken(Type expected, Token actual) => string.Format(_LEXER_ERROR_TEMPLATE, actual.Location.X, actual.Location.Y,
            string.Format(_EXPECTED_TOKEN_FORMAT, expected.Name, actual.GetType()));

        public static string UnexpectedToken(Token token) => string.Format(_LEXER_ERROR_TEMPLATE, token.Location.X, token.Location.Y,
            $"Unexpected token ({token.GetType()}).");

        public static string InvalidToken(Point location, Rune character) =>
            string.Format(_LEXER_ERROR_TEMPLATE, location.X, location.Y, $"Failed to read a valid token ({character}).");
    }
}
