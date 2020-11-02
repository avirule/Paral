#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Paral.Lexing;
using Paral.Lexing.Tokens;
using Paral.Parsing;
using Paral.Parsing.Nodes;

#endregion


namespace Paral
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Compiler.InitializeLogger();

            Lexer lexer = new Lexer(File.OpenRead("TestFiles/Test_Structs.paral"));

            List<Token>? tokens = await lexer.Tokenize().ToListAsync();

            Parser parser = new Parser(File.OpenRead("TestFiles/Test_Structs.paral"));
            MasterNode masterNode = await parser.Parse();
        }
    }
}
