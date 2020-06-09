#region

using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class ArgumentNode : Node
    {
        public RuntimeType? Type { get; private set; }
        public string? Name { get; private set; }

        public ArgumentNode()
        {
            Type = null;
            Name = null;
        }

        protected override bool ConsumeTokenInternal(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Identifier when Type == null:
                    Type = new RuntimeType(token.Value);
                    return false;
                case TokenType.Identifier when Name == null:
                    Name = token.Value;
                    return true; // all values have been set
                case TokenType.Identifier:
                    ExceptionHelper.Error(token, "Expected argument separator.");
                    return false;
                default:
                    ExceptionHelper.Error(token, "Invalid token.");
                    return false;
            }
        }
    }
}
