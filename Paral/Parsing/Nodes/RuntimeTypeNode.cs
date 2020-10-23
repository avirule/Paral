#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class RuntimeTypeNode : LeafNode
    {
        public string Value { get; }

        public RuntimeTypeNode(IdentifierToken identifierToken) => Value = identifierToken.Value;

        protected override bool ConsumeTokenInternal(Token token) => true;
    }
}
