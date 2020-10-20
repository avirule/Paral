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

        public async Task<ApplicationNode> Parse()
        {
            ApplicationNode applicationNode = new ApplicationNode();

            await foreach (Token token in _Lexer.Tokenize())
            {
                applicationNode.ConsumeToken(token);
            }

            return applicationNode;
        }
    }
}
