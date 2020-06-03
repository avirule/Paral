#region

using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class LocalityNode : BridgeNode
    {
        public LocalityIdentifier Locality { get; }

        public LocalityNode(LocalityIdentifier locality)
        {
            Locality = locality;
            Child = null;
        }

        protected override void AttemptInitializeChild(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Identifier when token.Value.Equals(KeywordHelper.CLASS):
                case TokenType.Identifier when token.Value.Equals(KeywordHelper.DATA):
                    break;
                case TokenType.Identifier: // we assume it's a type
                    Child = new RuntimeTypeNode(new RuntimeType(token.Value));
                    break;
                default:
                    ExceptionHelper.Error(token, "Expected identifier.");
                    break;
            }
        }
    }
}
