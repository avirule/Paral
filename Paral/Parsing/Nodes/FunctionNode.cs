#region

using System;
using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class FunctionNode : Node
    {
        public FunctionLocalityIdentifier Locality { get; private set; }
        public RuntimeType Type { get; private set; }
        public string Name { get; private set; }
        public CompoundArgumentsNode Arguments { get; private set; }
        public CompoundNode Body { get; private set; }

        public FunctionNode() => Locality = 0;

        public override void Consume(Token token)
        {
            if (Locality == 0)
            {
                if (token.Type != TokenType.Identifier)
                {
                    ExceptionHelper.Error(token, "Expected locality identifier.");
                }
                else if (!Enum.TryParse(token.Value, true, out FunctionLocalityIdentifier localityIdentifier))
                {
                    ExceptionHelper.Error(token, "Invalid locality identifier.");
                }
                else
                {
                    Locality = localityIdentifier;
                }
            }
            else if (Type == null)
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
                    ExceptionHelper.Error(token, "Expected function name.");
                }
                else
                {
                    Name = token.Value;

                    // todo register function somehow to avoid multiple declarations
                }
            }
            else if (Arguments == null)
            {
                if (token.Type != TokenType.ParenthesisOpen)
                {
                    ExceptionHelper.Error(token, "Expected opening parenthesis.");
                }
                else
                {
                    Arguments = new CompoundArgumentsNode();
                }
            }
            else if (!Arguments.Complete)
            {
                Arguments.Consume(token);
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
    }
}
