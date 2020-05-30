using System;
using System.Drawing;
using Serilog;

namespace Paral.Exceptions
{
    public class ExceptionHelper
    {
        public const string LEXER_ERROR_TEMPLATE = "[Row: {0}, Col: {1}] {2}";

        public static void Error(Point location, string error)
        {
            Log.Error(string.Format(LEXER_ERROR_TEMPLATE, location.X, location.Y, error));
            Environment.Exit(-1);
        }
    }
}
