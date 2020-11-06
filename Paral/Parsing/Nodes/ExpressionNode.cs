using Paral.Lexing;
using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class ExpressionNode : Node
    {
        public Node? Tail { get; private set; }

        protected override bool ConsumeTokenInternal(Token token)
        {
            if (Tail is null)
            {
                if (_Tokens.Count > 0)
                {
                    switch (_Tokens[^1])
                    {
                        case IdentifierToken:
                            if (token is not OperatorToken<Add> )


                            break;
                            case ILiteralToken: break;
                    }
                }

                switch (token)
                {
                    case LiteralToken<Numeric>:

                    default: throw new UnexpectedTokenException(token);
                }
            }
            else return Tail.ConsumeToken(token);
        }
    }
}
