#region

using Paral.Lexing.Tokens;
using Paral.Lexing.Tokens.Blocks;

#endregion


namespace Paral.Parsing.Nodes
{
    public class FunctionNode : Node
    {
        public RuntimeTypeNode? RuntimeType => Branches.Count > 0 ? Branches[0] as RuntimeTypeNode : null;
        public IdentifierNode? Identifier => Branches.Count > 1 ? Branches[1] as IdentifierNode : null;
        public ParametersNode? Parameters => Branches.Count > 2 ? Branches[2] as ParametersNode : null;
        public BlockNode? Logic => Branches.Count < 3 ? Branches[3] as BlockNode : null;

        public FunctionNode(IdentifierToken typeIdentifierToken) => Branches.Add(new RuntimeTypeNode(typeIdentifierToken));

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
                    if (token is IdentifierToken identifierToken) Branches.Add(new IdentifierNode(identifierToken));
                    else ThrowHelper.ThrowExpectedIdentifier(token);
                }
                else if (Parameters is null)
                {
                    if (token is ParenthesisToken parenthesisToken && (parenthesisToken.Intent == BlockTokenIntent.Open)) Branches.Add(new ParametersNode());
                    else ThrowHelper.Throw(token, "Expected parenthesis to open function parameters.");
                }
                else if (Logic is null)
                {
                    if (token is BracketToken bracketToken && (bracketToken.Intent == BlockTokenIntent.Open)) Branches.Add(new BlockNode());
                    else ThrowHelper.Throw(token, "Expected function body.");
                }
                else ThrowHelper.Throw(token, "Node is complete.");
            }

            return false;
        }
    }
}
