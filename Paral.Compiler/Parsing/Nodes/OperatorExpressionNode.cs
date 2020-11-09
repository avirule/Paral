using Paral.Compiler.Lexing.Tokens;
using Paral.Compiler.Parsing.Nodes.Functions;

namespace Paral.Compiler.Parsing.Nodes
{
    public class OperatorExpressionNode : ExpressionNode
    {
        public Node Left { get; }
        public Operator Operator { get; }

        public OperatorExpressionNode(Node left, Operator @operator) => (Left, Operator) = (left, @operator);
    }
}
