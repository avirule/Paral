#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class LiteralNode : LeafNode
    {
        public Literal Type { get; }
        public string Value { get; }

        public LiteralNode(Literal type, string value) => (Type, Value) = (type, value);
    }
}
