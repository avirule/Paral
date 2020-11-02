#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class LiteralNode<T> : LeafNode, ILiteralValued where T : ILiteral
    {
        public string Value { get; }

        public LiteralNode(string value) => Value = value;
    }
}
