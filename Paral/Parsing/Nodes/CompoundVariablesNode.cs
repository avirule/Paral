#region

using System.Collections.Generic;
using System.Drawing;
using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class CompoundVariablesNode : Node
    {
        public List<VariableNode> Variables { get; }

        public CompoundVariablesNode(Point location)
        {
            Location = location;
            Variables = new List<VariableNode>();
        }

        public override void Consume(Token token)
        {
            switch (token.Type)
            {
                case TokenType.ParenthesisClose:
                    if ((Variables.Count > 0) && !Variables[^1].Complete)
                    {
                        ExceptionHelper.Error(token, "Parenthesis closure invalid at this point.");
                    }
                    else
                    {
                        Complete = true;
                    }

                    break;
                case TokenType.ArgumentSeparator:
                    if ((Variables.Count == 0) || !Variables[^1].Complete)
                    {
                        ExceptionHelper.Error(token, "Argument separator invalid at this point.");
                    }
                    else
                    {
                        Variables.Add(new VariableNode());
                    }

                    break;
                case TokenType.Identifier:
                    if (Variables.Count == 0)
                    {
                        Variables.Add(new VariableNode());
                    }
                    else if (Variables[^1].Complete)
                    {
                        ExceptionHelper.Error(token, "Identifier invalid here (are you missing an argument separator?)");
                    }

                    Variables[^1].Consume(token);
                    break;
                default:
                    ExceptionHelper.Error(token, "Invalid token.");
                    break;
            }
        }
    }
}
