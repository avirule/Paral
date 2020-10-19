#region

using System;
using System.Drawing;
using Paral.Lexing;
using Paral.Lexing.Tokens;
using Serilog;

#endregion

namespace Paral.Exceptions
{
    public class ExceptionHelper
    {
        public const string LEXER_ERROR_TEMPLATE = "[Row: {0}, Col: {1}] {2}";
        public const string TOKEN_ERROR_TEMPLATE = "[Row: {0}, Col: {1}, {2}] {3}";
        public const string INVALID_COMPILER_STATE = "Compiler has entered invalid control flow.";
        public const string EXPECTED_IDENTIFIER = "Expected identifier.";

        public static void Error(string error)
        {
            Log.Error(error);
            Environment.Exit(-1);
        }

        public static void Error(Point location, string error)
        {
            Log.Error(string.Format(LEXER_ERROR_TEMPLATE, location.X, location.Y, error));
            Environment.Exit(-1);
        }

        public static void Error(Token token, string error)
        {
            Log.Error(string.Format(TOKEN_ERROR_TEMPLATE, token.Location.X, token.Location.Y, token.GetType(), error));
            Environment.Exit(-1);
        }
    }
}
