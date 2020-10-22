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
            if ((Leaves.Count > 0) && !Leaves[^1].Completed) Leaves[^1].ConsumeToken(token);
            else
            {
                switch (token)
                {
                    case IdentifierToken identifierToken:
                        Leaves.Add(new ParameterNode(identifierToken));
                        break;
                    case ParenthesisToken parenthesisToken when parenthesisToken.Intent == BlockTokenIntent.Close: return true;
                    default:
                        ThrowHelper.ThrowUnexpectedToken(token);
                        break;
                }
            }

            return false;
        }
    }
}
