#region

using System.IO;
using System.Threading.Tasks;
using Paral.Compiler.Lexing;
using Paral.Compiler.Lexing.Tokens;
using Paral.Compiler.Parsing.Nodes;

#endregion


namespace Paral.Compiler.Parsing
{
    public class Parser
    {
        private readonly Lexer _Lexer;

        public Parser(Stream data) => _Lexer = new Lexer(data);

        public async Task<Module> Parse()
        {
            Module module = new Module();

            await foreach (Token token in _Lexer.Tokenize()) module.ConsumeToken(token);

            return module;
        }
    }
}
