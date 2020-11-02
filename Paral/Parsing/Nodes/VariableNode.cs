#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class VariableNode<T> : Node where T : IMutability
    {
        public IdentifierNode? Identifier => Branches.Count > 0 ? Branches[0] as IdentifierNode : null;
        public RuntimeTypeNode? RuntimeType => Branches.Count > 1 ? Branches[1] as RuntimeTypeNode : null;
        public Node? Value => Branches.Count > 2 ? Branches[2] : null;

        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed) return Branches[^1].ConsumeToken(token);
            else
            {
                if (Identifier is null)
                {
                    if (token is IdentifierToken identifierToken) Branches.Add(new IdentifierNode(identifierToken.Value));
                    else ThrowHelper.ThrowExpectedIdentifier(token);
                }
                else if (RuntimeType is null)
                {
                    if (token is IdentifierToken identifierToken) Branches.Add(new RuntimeTypeNode(identifierToken));
                    else ThrowHelper.ThrowExpectedIdentifier(token);
                }
                else if (Value is null) Branches.Add(new ExpressionNode());
                else ThrowHelper.ThrowUnexpectedToken(token);
            }

            return false;
        }
    }
}
