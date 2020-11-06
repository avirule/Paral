using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class RequiresNode : Node
    {
        protected override bool ConsumeTokenInternal(Token token)
        {
            if (_Tokens.Count == 0)
            {
                Expect<IdentifierToken>(token);
                _Tokens.Add(token);
            }
            else
            {
                switch (_Tokens[^1])
                {
                    case SeparatorToken<Path>:
                        Expect<IdentifierToken>(token);
                        _Tokens.Add(token);
                        break;
                    case IdentifierToken:
                        Expect<SeparatorToken<Path>, TerminatorToken>(token);

                        switch (token)
                        {
                            case SeparatorToken<Path>:
                                _Tokens.Add(token);
                                break;
                            case TerminatorToken: return true;
                        }

                        return false;
                }
            }

            return false;
        }
    }
}
