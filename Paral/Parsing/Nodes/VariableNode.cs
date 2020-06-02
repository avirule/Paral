#region

using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class VariableNode : Node
    {
        public RuntimeType Type { get; private set; }
        public string Name { get; private set; }

        public override void Consume(Token token)
        {
            if (Type == null)
            {
                if (token.Type != TokenType.Identifier)
                {
                    ExceptionHelper.Error(token, "Expected type identifier.");
                }
                else
                {
                    Location = token.Location;
                    Type = new RuntimeType(token.Value);
                    TypeChecker.UsedTypes.Add(Type);
                }
            }
            else if (string.IsNullOrEmpty(Name))
            {
                if (token.Type != TokenType.Identifier)
                {
                    ExceptionHelper.Error(token, "Expected parameter name.");
                }
                else
                {
                    Name = token.Value;
                    Complete = true;
                }
            }is
            else
            {
                ExceptionHelper.Error(token, "Received invalid token.");
            }
        }
    }
}
