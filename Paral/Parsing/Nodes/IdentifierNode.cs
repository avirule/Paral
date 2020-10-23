#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class IdentifierNode : LeafNode
    {
        public string Value { get; }

        public IdentifierNode(IdentifierToken identifierToken) => Value = identifierToken.Value;
    }
}
