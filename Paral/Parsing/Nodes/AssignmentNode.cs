#region

using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class AssignmentNode : BridgeNode
    {
        protected override void AttemptInitializeChild(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Identifier when token.Value.Equals(KeywordHelper.ARGUMENTS):
                    Child = new LogicalNode();
                    break;
                default:
                    ExceptionHelper.Error(token, "Invalid token at this point.");
                    break;
            }
        }
    }
}
