#region

using System.IO;
using System.Threading.Tasks;
using Paral.Lexing;
using Paral.Parsing.Nodes;

#endregion

namespace Paral.Parsing
{
    public class Parser
    {
        private readonly Lexer _Lexer;

        public RootNode RootNode { get; }

        public Parser(Stream data)
        {
            _Lexer = new Lexer(data);

            RootNode = new RootNode();
        }

        public async Task Parse()
        {
            await foreach (Token token in _Lexer.Tokenize())
            {
                RootNode.Consume(token);
            }
        }
    }
}
