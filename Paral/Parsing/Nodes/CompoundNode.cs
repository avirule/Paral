#region

using System.Collections.Generic;
using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class CompoundNode : Node
    {
        private readonly TokenType _Opener;
        private readonly List<Node> _Children;

        public CompoundNode(TokenType opener)
        {
            _Opener = opener;
            _Children = new List<Node>();
        }

        public override void Consume(Token token)
        {
            switch (token.Type)
            {
                case TokenType.ParenthesisClose when _Opener == TokenType.ParenthesisOpen:
                case TokenType.CurlyBracketClose when _Opener == TokenType.CurlyBracketOpen:
                    Complete = true;
                    break;
                default:
                    ExceptionHelper.Error(token, "Invalid token.");
                    break;
            }
        }
    }
}
