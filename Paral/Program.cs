#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Paral.Lexing;

#endregion

namespace Paral
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            Lexer lexer = new Lexer(File.OpenRead("Test.paral"));
            List<Token> tokens = await lexer.Tokenize().ToListAsync();
            //Compiler compiler = new Compiler(args);
            //await compiler.Compile();
        }
    }
}
