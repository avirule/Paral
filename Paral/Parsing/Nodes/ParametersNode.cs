using System.Collections.Generic;
using Paral.Lexing;
using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class ParameterNode : Node
    {
        public string? TypeIdentifier { get; private set; }
        public string? Identifier { get; private set; }
        public bool Separated { get; set; }

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
            if ((Parameters.Count > 0) && !Parameters[^1].Completed) Parameters[^1].ConsumeToken(token);
            else
            {
                if ((Parameters.Count == 0) || Parameters[^1].Separated)
                {
                    Expect<IdentifierToken>(token);
                    Parameters.Add(new ParameterNode());
                    Parameters[^1].ConsumeToken(token);
                }
                else
                {
                    Expect(token, typeof(GroupToken<Parenthetic, Close>), typeof(SeparatorToken<Comma>));

                    switch (token)
                    {
                        case SeparatorToken<Comma> when !Parameters[^1].Separated:
                            Parameters[^1].Separated = true;
                            break;
                        case GroupToken<Parenthetic, Close> when !Parameters[^1].Separated: return true;
                    }
                }
            }

            return false;
        }
    }
}
