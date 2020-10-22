#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class ParameterNode : Node
    {
        public IdentifierNode? Identifier => Branches[0] as IdentifierNode;
        public RuntimeTypeNode? Type => Branches[1] as RuntimeTypeNode;

        public ParameterNode(IdentifierToken identifierToken) => Branches.Add(new IdentifierNode(identifierToken));

        protected override bool ConsumeTokenInternal(Token token)
        {
            switch (token)
            {
                case IdentifierToken identifierToken:
                    Branches.Add(new RuntimeTypeNode(identifierToken));
                    return true;
                default:
                    ThrowHelper.ThrowUnexpectedToken(token);
                    return false;
            }
        }
    }
}
