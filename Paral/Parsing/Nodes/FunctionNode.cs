#region

using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class FunctionNode : Node
    {
        public RuntimeType? Type { get; private set; }
        public string Name { get; private set; }
        public CompoundVariablesNode? Variables { get; private set; }
        public CompoundNode? Body { get; private set; }

        public FunctionNode()
        {
            Type = null;
            Name = string.Empty;
            Variables = null;
            Body = null;
        }

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
                AttemptInitializeVariables(token);
            }
            else if (!Variables.Complete)
            {
                Variables.Consume(token);
            }
            else if (Body == null)
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

        private void AttemptInitializeVariables(Token token)
        {
            if (token.Type != TokenType.ParenthesisOpen)
            {
                ExceptionHelper.Error(token, "Expected opening parenthesis.");
            }
            else
            {
                Variables = new CompoundVariablesNode(token.Location);
            }
        }
    }
}
