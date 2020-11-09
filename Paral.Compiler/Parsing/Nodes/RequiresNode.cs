using Paral.Compiler.Lexing.Tokens;

namespace Paral.Compiler.Parsing.Nodes
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
                        Expect(token, typeof(SeparatorToken<Path>), typeof(TerminatorToken));

                        if (token is SeparatorToken<Path>) _Tokens.Add(token);
                        else if (token is TerminatorToken) return true;

                        break;
                }
            }

            return Completed;
        }
    }
}
