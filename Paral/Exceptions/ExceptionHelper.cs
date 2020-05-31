#region

using System;
using System.Drawing;
using Paral.Lexing;
using Serilog;

#endregion

namespace Paral.Exceptions
{
    public class ExceptionHelper
    {
        public const string LEXER_ERROR_TEMPLATE = "[Row: {0}, Col: {1}] {2}";
        public const string TOKEN_ERROR_TEMPLATE = "[Row: {0}, Col: {1}, {2}] {3}";

        public static void Error(Point location, string error)
        {
            Log.Error(string.Format(LEXER_ERROR_TEMPLATE, location.X, location.Y, error));
            Environment.Exit(-1);
        }

        public static void Error(Token token, string error)
        {
            Log.Error(string.Format(TOKEN_ERROR_TEMPLATE, token.Location.X, token.Location.Y,
                IsValuedToken(token) ? $"{token.Type}: {token.Value}" : token.Type.ToString(), error));
            Environment.Exit(-1);
        }

        private static bool IsValuedToken(Token token) => token.Type switch
        {
            TokenType.EndOfFile => false,
            _ => true
        };
    }
}
