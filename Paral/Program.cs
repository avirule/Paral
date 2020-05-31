#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Paral.Lexing;
using Serilog;

#endregion

namespace Paral
{
    internal class Program
    {
        private const string _DEFAULT_TEMPLATE = "{Timestamp:MM/dd/yy-HH:mm:ss} | {Level:u3} | {Message}\r\n";
        private const string _FILE_NAME = "Test.paral";

        private static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration().WriteTo.Console(outputTemplate: _DEFAULT_TEMPLATE).CreateLogger();


            FileStream fs = File.OpenRead(_FILE_NAME);
            Lexer lexer = new Lexer(fs);
            List<Token> output = new List<Token>();
            await foreach (Token token in lexer.Tokenize())
            {
                output.Add(token);
            }


            //Compiler compiler = new Compiler(_FILE_NAME);
            //compiler.Compile();

            // Log.Information
            // (
            //     $"Compiled file \"{_FILE_NAME}\" in (parser: {compiler.ParserTime.TotalMilliseconds:0.00}ms, overall: {compiler.CompileTime.TotalMilliseconds:0.00}ms)."
            // );
        }
    }
}
