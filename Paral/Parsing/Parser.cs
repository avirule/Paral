#region

using System.IO;
using System.Text;
using System.Threading.Tasks;
using Paral.Lexing;
using Paral.Lexing.Tokens;
using Paral.Parsing.Nodes;

#endregion

namespace Paral.Parsing
{
    public class Parser
    {
        private readonly Lexer _Lexer;

        public Parser(Stream data)
        {
            _Lexer = new Lexer(data);

        }

        public async Task<MasterNode> Parse()
        {
            MasterNode masterNode = new MasterNode();

            await foreach (Token token in _Lexer.Tokenize())
            {
                masterNode.ConsumeToken(token);
            }

            return masterNode;
        }
    }
}
