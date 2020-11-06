#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class FunctionNode : Node
    {
        public IdentifierNode? Identifier => Branches.Count > 0 ? Branches[0] as IdentifierNode : null;
        public ParametersNode? Parameters => Branches.Count > 1 ? Branches[1] as ParametersNode : null;
        public RuntimeTypeNode? RuntimeType => Branches.Count > 2 ? Branches[2] as RuntimeTypeNode : null;
        public BlockNode? Logic => Branches.Count < 3 ? Branches[3] as BlockNode : null;

        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed)
            {
                Node node = Branches[^1];
                return node.ConsumeToken(token) && node is BlockNode;
            }
            else
            {
                if (Identifier is null)
                {
                    if (token is IdentifierToken identifierToken) Branches.Add(new IdentifierNode(identifierToken.Value));
                    else ThrowHelper.ThrowExpectedIdentifier(token);
                }
                else if (Parameters is null)
                {
                    if (token is GroupToken<Parenthetic, Open>) Branches.Add(new ParametersNode());
                    else ThrowHelper.Throw(token, "Expected parenthesis to open function parameters.");
                }
                else if (RuntimeType is null)
                {
                    if (token is IdentifierToken identifierToken) Branches.Add(new RuntimeTypeNode(identifierToken));
                    else ThrowHelper.ThrowExpectedIdentifier(token);
                }
                else if (Logic is null)
                {
                    if (token is GroupToken<Bracket, Open>) Branches.Add(new BlockNode());
                    else ThrowHelper.Throw(token, "Expected function body.");
                }
                else ThrowHelper.Throw(token, "Node is complete.");
            }

            return false;
        }
    }
}
