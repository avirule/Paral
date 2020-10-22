#region

using Paral.Lexing.Tokens;
using Paral.Lexing.Tokens.Blocks;

#endregion


namespace Paral.Parsing.Nodes
{
    public class ValueNode : Node
    {
        public IdentifierToken Identifier { get; }

        public ValueNode(IdentifierToken identifierToken) => Identifier = identifierToken;

        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Leaves.Count > 0) && !Leaves[^1].Completed)
            {
                Node node = Leaves[^1];
                return node.ConsumeToken(token) && node is BlockNode;
            }
            else
            {
                switch (token)
                {
                    case ParenthesisToken parenthesisToken when parenthesisToken.Intent == BlockTokenIntent.Open:
                        Leaves.Add(new ParametersNode());
                        return false;
                }
            }

            return false;
        }
    }
}
