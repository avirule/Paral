#region

using Paral.Lexing.Tokens;
using Paral.Lexing.Tokens.Blocks;

#endregion


namespace Paral.Parsing.Nodes
{
    public class BlockNode : Node
    {
        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed) Branches[^1].ConsumeToken(token);
            else
            {
                switch (token)
                {
                    case BracketToken bracketToken when bracketToken.Intent == BlockTokenIntent.Close: return true;
                }
            }

            return false;
        }
    }
}
