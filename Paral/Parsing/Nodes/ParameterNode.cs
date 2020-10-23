#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class ParameterNode : Node
    {
        public RuntimeTypeNode? RuntimeType => Branches.Count > 0 ? Branches[0] as RuntimeTypeNode : null;
        public IdentifierNode? Identifier => Branches.Count > 1 ? Branches[1] as IdentifierNode : null;

        public ParameterNode(IdentifierToken identifierToken) => Branches.Add(new RuntimeTypeNode(identifierToken));

        protected override bool ConsumeTokenInternal(Token token)
        {
            switch (token)
            {
                case IdentifierToken identifierToken:
                    Branches.Add(new IdentifierNode(identifierToken));
                    return true;
                default:
                    ThrowHelper.ThrowUnexpectedToken(token);
                    return false;
            }
        }
    }
}
