#region

using System.Drawing;
using System.IO;
using Serilog;

#endregion

namespace Paral
{
    public class Token
    {
        public Point Location { get; }
        public TokenType TokenType { get; }
        public string Value { get; }

        public Token(Point location, TokenType tokenType, string value) => (Location, TokenType, Value) = (location, tokenType, value);
    }

    internal class Program
    {
        private const string _DEFAULT_TEMPLATE = "{Timestamp:MM/dd/yy-HH:mm:ss} | {Level:u3} | {Message}\r\n";

        private static void Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console(outputTemplate: _DEFAULT_TEMPLATE).CreateLogger();

            LexicalAnalyzer lexicalAnalyzer = new LexicalAnalyzer(File.ReadAllText("Test.paral").ToCharArray());
            lexicalAnalyzer.Tokenize();
        }
    }
}
