using System.Collections.Generic;
using Paral.Compiler.Lexing;
using Paral.Compiler.Lexing.Tokens;

namespace Paral.Compiler.Parsing.Nodes.Functions
{
    public class ParameterNode : Node
    {
        public Mutability Mutability { get; }
        public string? TypeIdentifier { get; private set; }
        public string? Identifier { get; private set; }
        public bool Separated { get; set; }

        public ParameterNode(Mutability mutability) => Mutability = mutability;

        protected override bool ConsumeTokenInternal(Token token)
        {
            if (TypeIdentifier is null)
            {
                Expect<IdentifierToken>(token);
                TypeIdentifier = (token as IdentifierToken)!.Value;

                return false;
            }
            else if (Identifier is null)
            {
                Expect<IdentifierToken>(token);
                Identifier = (token as IdentifierToken)!.Value;

                return true;
            }
            else throw new UnexpectedTokenException(token);
        }
    }

    public class ParametersNode : Node
    {
        public List<ParameterNode> Parameters { get; }

        public ParametersNode() => Parameters = new List<ParameterNode>();

        protected override bool ConsumeTokenInternal(Token token)
        {
            switch (Parameters.Count)
            {
                case 0:
                    Expect<MutabilityToken>(token);
                    Parameters.Add(new ParameterNode((token as MutabilityToken)!.Mutability));
                    break;

                case > 0 when !Parameters[^1].Completed:
                    Parameters[^1].ConsumeToken(token);
                    break;

                default:
                    switch (token)
                    {
                        case MutabilityToken mutabilityToken when LastSavedTokenIs<SeparatorToken<Comma>>():
                            Parameters.Add(new ParameterNode(mutabilityToken.Mutability));
                            _Tokens.Clear();
                            break;
                        case SeparatorToken<Comma> when _Tokens.Count == 0:
                            _Tokens.Add(token);
                            break;
                        case GroupToken<Paren, Close> when _Tokens.Count == 0: return true;
                        default: throw new UnexpectedTokenException(token);
                    }

                    break;
            }

            return Completed;
        }
    }
}
