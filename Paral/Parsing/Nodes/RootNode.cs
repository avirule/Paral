#region

using System.Collections.Generic;
using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class RootNode : Node
    {
        private readonly List<Node> _Children;

        public RootNode() => _Children = new List<Node>();

        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((_Children.Count > 0) && !_Children[^1].Complete)
            {
                return _Children[^1].ConsumeToken(token);
            }
            else
            {
                switch (token.Type)
                {
                    case TokenType.Identifier when token.Value.Equals(KeywordHelper.REQUIRES):
                        // todo code rules for adding namespace requires
                        break;
                    case TokenType.Identifier when token.Value.Equals(KeywordHelper.PUBLIC):
                        _Children.Add(new LocalityNode(LocalityIdentifier.Public));
                        break;
                    case TokenType.Identifier when token.Value.Equals(KeywordHelper.PRIVATE):
                        _Children.Add(new LocalityNode(LocalityIdentifier.Private));
                        break;
                    default:
                        ExceptionHelper.Error(token, ExceptionHelper.EXPECTED_IDENTIFIER);
                        break;
                }

                return false;
            }
        }
    }
}
