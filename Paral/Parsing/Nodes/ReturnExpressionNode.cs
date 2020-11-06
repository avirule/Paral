using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class ReturnExpressionNode : Node
    {
        public ExpressionNode Expression { get; }

        public ReturnExpressionNode() => Expression = new ExpressionNode();

        protected override bool ConsumeTokenInternal(Token token) => Expression.ConsumeToken(token);
    }
}
