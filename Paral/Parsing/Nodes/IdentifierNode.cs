using System.Reflection.Metadata.Ecma335;
using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class IdentifierNode : LeafNode
    {
        public string Value { get; }

        public IdentifierNode(IdentifierToken identifierToken) => Value = identifierToken.Value;

        protected override bool ConsumeTokenInternal(Token token) => true;
    }
}
