#region

using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class ExpressionNode : Node
    {
        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed) Branches[^1].ConsumeToken(token);
            else
            {
                switch (token)
                {
                    case LiteralToken<Numeric>: break;
                    case TerminatorToken: return true;
                }
            }

            return false;
        }
    }
}
