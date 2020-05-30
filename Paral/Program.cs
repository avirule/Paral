#region

using System.IO;
using Serilog;

#endregion

namespace Paral
{
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
