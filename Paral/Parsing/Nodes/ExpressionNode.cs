#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public interface ILiteralValued
    {
        public string Value { get; }
    }

    public class ExpressionNode : Node
    {
        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed) return Branches[^1].ConsumeToken(token);
            else
            {
                switch (token)
                {
                    case IdentifierToken identifierToken:
                        Branches.Add(new IdentifierNode(identifierToken.Value));
                        break;
                    case LiteralToken<Numeric> literalToken:
                        Branches.Add(new LiteralNode<Numeric>(literalToken.Value));
                        break;
                    case ArithmeticToken arithmeticToken when Branches[^1] is ILiteralValued:
                        Node node = Branches[^1];
                        Branches.RemoveAt(Branches.Count - 1);
                        ArithmeticNode arithmeticNode = new ArithmeticNode(arithmeticToken.Operator, node);
                        arithmeticNode.Branches.Add(node);
                        Branches.Add(arithmeticNode);
                        break;
                    default:
                        ThrowHelper.ThrowUnexpectedToken(token);
                        break;
                }
            }

            return false;
        }
    }
}
