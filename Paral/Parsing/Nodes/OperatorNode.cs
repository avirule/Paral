#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class OperatorNode<T> : LeafNode where T : IOperator { }
}
