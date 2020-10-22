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
}
