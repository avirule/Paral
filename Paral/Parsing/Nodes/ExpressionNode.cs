using Paral.Lexing;
using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class ReturnExpressionNode : ExpressionNode { }

    public class ExpressionNode : Node
    {
        public Node? Expression { get; private set; }

        protected override bool ConsumeTokenInternal(Token token) => Expression?.ConsumeToken(token) ?? ParseExpression(token);

        private bool ParseExpression(Token token)
        {
            if (_Tokens.Count > 0)
            {
                switch (_Tokens[^1])
                {
                    // termination statements
                    case LiteralToken literalToken when token is TerminatorToken:
                        Expression = new LiteralNode(literalToken.Type, literalToken.Value);
                        return true;
                    case IdentifierToken identifierToken when token is TerminatorToken:
                        Expression = new VariableReferenceNode(identifierToken.Value);
                        return true;

                    // operator statements
                    case LiteralToken literalToken when token is OperatorToken operatorToken:
                        Expression = new OperatorExpressionNode(new LiteralNode(literalToken.Type, literalToken.Value), operatorToken.Operator);
                        break;
                    case IdentifierToken identifierToken when token is OperatorToken operatorToken:
                        Expression = new OperatorExpressionNode(new VariableReferenceNode(identifierToken.Value), operatorToken.Operator);
                        break;
                    default: throw new UnexpectedTokenException(token);
                }
            }
            else
            {
                switch (token)
                {
                    case LiteralToken:
                    case IdentifierToken:
                        _Tokens.Add(token);
                        break;
                    default: throw new UnexpectedTokenException(token);
                }
            }

            return Completed;
        }
    }
}
