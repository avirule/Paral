#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class ApplicationNode : Node
    {
        public RequiresNode RequiresNode { get; }

        public ApplicationNode() => RequiresNode = new RequiresNode();

        protected override bool ConsumeTokenInternal(Token token)
        {
            if (!RequiresNode.Complete)
            {
                RequiresNode.ConsumeToken(token);
                return false;
            }
            else if (token is KeywordToken keywordToken && (keywordToken.Value == Keyword.Requires))
            {
                RequiresNode.ConsumeToken(token);
                return false;
            }

            return false;
        }
    }
}
