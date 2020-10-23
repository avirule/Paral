#region

using Paral.Lexing.Tokens;
using Paral.Lexing.Tokens.Blocks;

#endregion


namespace Paral.Parsing.Nodes
{
    public class ParametersNode : Node
    {
        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed) Branches[^1].ConsumeToken(token);
            else
            {
                switch (token)
                {
                    case IdentifierToken identifierToken:
                        Branches.Add(new ParameterNode(identifierToken));
                        break;
                    case ParenthesisToken parenthesisToken when parenthesisToken.Intent == BlockTokenIntent.Close: return true;
                    case SeparatorToken: break;
                    default:
                        ThrowHelper.ThrowUnexpectedToken(token);
                        break;
                }
            }

            return false;
        }
    }

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
