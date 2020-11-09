using Paral.Lexing.Tokens;
using Paral.Parsing.Nodes.Functions;

namespace Paral.Parsing.Nodes
{
    public class OperatorExpressionNode : ExpressionNode
    {
        public Node Left { get; }
        public Operator Operator { get; }

        public OperatorExpressionNode(Node left, Operator @operator) => (Left, Operator) = (left, @operator);
    }
}
