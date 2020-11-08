namespace Paral.Parsing.Nodes
{
    public class VariableReferenceNode : LeafNode
    {
        public string Value { get; }

        public VariableReferenceNode(string value) => Value = value;
    }
}
