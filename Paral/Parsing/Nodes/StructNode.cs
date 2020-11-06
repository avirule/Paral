#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class StructNode : Node
    {
        public IdentifierNode? Identifier => Branches.Count > 0 ? Branches[0] as IdentifierNode : null;
        public BlockNode? Body => Branches.Count > 1 ? Branches[1] as BlockNode : null;

        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed)
            {
                Node node = Branches[^1];
                return Branches[^1].ConsumeToken(token) && node is BlockNode;
            }
            else
            {
                if (Identifier is null)
                {
                    if (token is IdentifierToken identifierToken) Branches.Add(new IdentifierNode(identifierToken.Value));
                    else ThrowHelper.ThrowExpectedIdentifier(token);
                }
                else if (Body is null)
                {
                    if (token is GroupToken<Bracket, Open>) Branches.Add(new BlockNode());
                    else ThrowHelper.Throw(token, "Expected struct body.");
                }
                else ThrowHelper.Throw(token, "Node is complete.");
            }

            return false;
        }
    }
}
