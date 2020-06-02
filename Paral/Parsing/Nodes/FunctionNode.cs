#region

using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class FunctionNode : Node
    {
        public RuntimeType Type { get; private set; }
        public string Name { get; private set; }
        public CompoundVariablesNode Variables { get; private set; }
        public CompoundNode Body { get; private set; }

        public override void Consume(Token token)
        {
            if (Type == null)
            {
                AttemptInitializeType(token);
            }
            else if (string.IsNullOrEmpty(Name))
            {
                AttemptInitializeName(token);
            }
            else if (Variables == null)
            {
                Variables = new CompoundVariablesNode(token.Location);
            }
            else if (!Variables.Complete)
            {
                Variables.Consume(token);
            }
            if (Body == null)
            {
                if (token.Type != TokenType.CurlyBracketOpen)
                {
                    ExceptionHelper.Error(token, "Expected opening curly bracket.");
                }
                else
                {
                    Body = new CompoundNode(token.Type);
                }
            }
            else
            {
                Body.Consume(token);
                Complete = Body.Complete;
            }
        }

        private void AttemptInitializeType(Token token)
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

        private void AttemptInitializeName(Token token)
        {
            if (token.Type != TokenType.Identifier)
            {
                ExceptionHelper.Error(token, "Expected function name.");
            }
            else
            {
                Name = token.Value;

                // todo register function somehow to avoid multiple declarations
            }
        }
    }
}
