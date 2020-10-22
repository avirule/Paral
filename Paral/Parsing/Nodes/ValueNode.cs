#region

using Paral.Lexing.Tokens;
using Paral.Lexing.Tokens.Blocks;

#endregion


namespace Paral.Parsing.Nodes
{
    public class ValueNode : Node
    {
        public IdentifierNode? Identifier => Branches[0] as IdentifierNode;

        public ValueNode(IdentifierToken identifierToken) => Branches.Add(new IdentifierNode(identifierToken));

        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed)
            {
                Node node = Branches[^1];
                return node.ConsumeToken(token) && node is BlockNode;
            }
            else
            {
                switch (token)
                {
                    case ParenthesisToken parenthesisToken when parenthesisToken.Intent == BlockTokenIntent.Open:
                        Branches.Add(new ParametersNode());
                        break;
                    case IdentifierToken identifierToken:
                        if ((Branches.Count == 2) && Branches[1] is ParametersNode) Branches.Add(new RuntimeTypeNode(identifierToken));
                        else ThrowHelper.Throw(token, "Unexpected identifier token.");

                        break;
                }
            }

            return false;
        }
    }
}
