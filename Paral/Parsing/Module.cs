using System;
using System.Collections.Generic;
using System.Linq;
using Paral.Lexing;
using Paral.Lexing.Tokens;
using Paral.Parsing.Nodes;

namespace Paral.Parsing
{
    public class Module : BranchNode
    {
        private NamespaceNode? _CurrentNamespace;

        public List<RequiresNode> Requires { get; }

        public Module() => Requires = new List<RequiresNode>();

        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Requires.Count > 0) && !Requires[^1].Completed) Requires[^1].ConsumeToken(token);
            else if (_Tokens.Count > 0)
            {
                switch (_Tokens[^1])
                {
                    case KeywordToken<Namespace>:
                    case SeparatorToken<Path>:
                        Expect<IdentifierToken>(token);
                        _Tokens.Add(token);
                        break;
                    case IdentifierToken:
                        Expect(token, typeof(SeparatorToken<Path>), typeof(TerminatorToken));

                        switch (token)
                        {
                            case SeparatorToken<Path>:
                                _Tokens.Add(token);
                                break;
                            case TerminatorToken:
                                IEnumerable<string> identifiers = _Tokens.Where(tok => tok is IdentifierToken)
                                    .Select(tok => (tok as IdentifierToken)!.Value);

                                _CurrentNamespace = FindOrCreateNamespace(new Queue<string>(identifiers));
                                _Tokens.Clear();
                                break;
                        }

                        break;
                    default: throw new UnexpectedTokenException(token);
                }
            }
            else
            {
                switch (token)
                {
                    case KeywordToken<Namespace>:
                        if (_CurrentNamespace is null || _CurrentNamespace.IsComplete()) _Tokens.Add(token);
                        else throw new Exception("Previous namespace not in a complete state.");

                        break;
                    case KeywordToken<Requires>:
                        Requires.Add(new RequiresNode());
                        break;
                    default:
                        if (_CurrentNamespace is null) throw new NullReferenceException("No namespace has been declared.");

                        _CurrentNamespace.ConsumeToken(token);
                        break;
                }
            }

            // todo return a useful value
            return false;
        }
    }
}
