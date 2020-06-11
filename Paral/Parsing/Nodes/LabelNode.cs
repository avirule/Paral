#region

using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class LabelNode : BridgeNode
    {
        public string Label { get; }

        public LabelNode(string identifier) => Label = identifier;

        protected override void AttemptInitializeChild(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Assigment:

                    break;
                case TokenType.StatementClosure:
                case TokenType.ArgumentSeparator:
                    Child = new ClosureNode();
                    break;
                default:
                    ExceptionHelper.Error(token, "Expected statement closure.");
                    break;
            }
        }
    }
}
