#region

using System.Collections.Generic;
using Paral.Exceptions;
using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class RequiresNode : Node
    {
        public List<RequireNode> Requires { get; }

        public RequiresNode() => Requires = new List<RequireNode>();

        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Requires.Count > 0) && !Requires[^1].Complete) return Requires[^1].ConsumeToken(token);
            else if (token is KeywordToken keywordToken && (keywordToken.Value == Keyword.Requires))
            {
                Requires.Add(new RequireNode());
                return false;
            }
            else
            {
                ThrowHelper.ThrowUnexpectedToken(token);
                return false;
            }
        }
    }
}
