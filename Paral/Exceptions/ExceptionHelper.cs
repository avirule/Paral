#region

using System;
using Paral.Lexing;
using Serilog;

#endregion

namespace Paral.Exceptions
{
    public class ExceptionHelper
    {
        public const string LEXER_ERROR_TEMPLATE = "[Row: {0}, Col: {1}, {2}: {3}] {4}";

        public static void Error(Token token, string error)
        {
            Log.Error(string.Format(LEXER_ERROR_TEMPLATE, token.Location.X, token.Location.Y, token.Type, token.Value, error));
            Environment.Exit(-1);
        }
    }
}
