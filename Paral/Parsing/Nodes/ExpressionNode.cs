#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class ExpressionNode : Node
    {
        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed)
            {
                Branches[^1].ConsumeToken(token);
            }
            else
            {
                switch (token)
                {
                    case IdentifierToken identifierToken:
                        Branches.Add(new IdentifierNode(identifierToken));
                        break;
                    case LiteralToken literalToken:
                        Branches.Add(new LiteralNode(literalToken.Type, literalToken.Value));
                        break;
                    case ArithmeticToken arithmeticToken:
                        if (Branches[^1] is LiteralNode or IdentifierNode)
                        {
                            Node node = Branches[^1];
                            Branches.RemoveAt(Branches.Count - 1);
                            ArithmeticNode arithmeticNode = new ArithmeticNode(arithmeticToken.Operator);
                            arithmeticNode.Branches.Add(node);
                        }
                        else
                        {
                            ThrowHelper.ThrowUnexpectedToken(token);
                        }

                        break;
                    case TerminatorToken: return true;
                }
            }

            return false;
        }
    }
}
