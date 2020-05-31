#region

using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class ArgumentNode : Node
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
            }
            else if (Complete)
            {
                ExceptionHelper.Error(token, "Received invalid argument token (are you missing a parenthesis closure?).");
            }
            else
            {
                ExceptionHelper.Error(token, "Compiler has reached invalid control flow path. This is likely a compiler error.");
            }
        }
    }
}
