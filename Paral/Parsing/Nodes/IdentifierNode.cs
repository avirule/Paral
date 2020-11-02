#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class IdentifierNode : LeafNode, ILiteralValued
    {
        public string Value { get; }

        public IdentifierNode(string identifier) => Value = identifier;
    }
}
