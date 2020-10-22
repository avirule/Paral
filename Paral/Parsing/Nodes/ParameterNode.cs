#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class ParameterNode : Node
    {
        public IdentifierToken Identifier { get; }
        public IdentifierToken? Type { get; private set; }

        public ParameterNode(IdentifierToken identifier) => Identifier = identifier;

        protected override bool ConsumeTokenInternal(Token token)
        {
            switch (token)
            {
                case IdentifierToken identifierToken:
                    Type = identifierToken;
                    return true;
                default:
                    ThrowHelper.ThrowUnexpectedToken(token);
                    return false;
            }
        }
    }
}
