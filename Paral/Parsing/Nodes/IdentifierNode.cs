#region

using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class IdentifierNode : BridgeNode
    {
        public string Identifier { get; }

        public IdentifierNode(string identifier) => Identifier = identifier;

        protected override void AttemptInitializeChild(Token token)
        {
            switch (token.Type)
            {
                case TokenType.StatementClosure:

                    break;
                default:
                    ExceptionHelper.Error(token, "Expected statement closure.");

            }
        }
    }
}
