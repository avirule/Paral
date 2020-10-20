#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Paral.Lexing;
using Paral.Lexing.Tokens;

#endregion

namespace Paral
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Compiler.InitializeLogger();

            Lexer lexer = new Lexer(File.OpenRead("Test_Heavy.paral"));
            List<Token> tokens = await lexer.Tokenize().ToListAsync();
            //Compiler compiler = new Compiler(args);
            //await compiler.Compile();
        }
    }
}
