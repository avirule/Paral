#region

using System;
using Paral.Lexing.Tokens;
using String = Paral.Lexing.Tokens.String;

#endregion


namespace Paral.Parsing.Nodes
{
    public class ArithmeticNode : Node
    {
        public ArithmeticOperator Operator { get; }

        public ArithmeticNode(ArithmeticOperator @operator, Node node)
        {
            if (node is not ILiteralValued)
            {
                throw new ArgumentException(nameof(node),
                    $"Node must be of type {nameof(ILiteralValued)}. This is a compiler error.");
            }

            Operator = @operator;
            Branches.Add(node);
        }

        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed) return Branches[^1].ConsumeToken(token);
            else
            {
                switch (token)
                {
                    case LiteralToken<Numeric> literalToken:
                        Branches.Add(new LiteralNode<Numeric>(literalToken.Value));
                        break;
                    case LiteralToken<Character> literalToken:
                        Branches.Add(new LiteralNode<Character>(literalToken.Value));
                        break;
                    case LiteralToken<String> literalToken:
                        Branches.Add(new LiteralNode<String>(literalToken.Value));
                        break;
                    case IdentifierToken identifierToken:
                        Branches.Add(new IdentifierNode(identifierToken.Value));
                        break;
                    case TerminatorToken when Branches.Count == 2: return true;
                    default:
                        ThrowHelper.ThrowUnexpectedToken(token);
                        break;
                }
            }

            return false;
        }
    }
}
