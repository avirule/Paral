using Paral.Lexing;
using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class BlockNode : BranchNode
    {
        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed) Branches[^1].ConsumeToken(token);
            else
            {
                switch (token)
                {
                    case KeywordToken<Return>:
                        Branches.Add(new ReturnExpressionNode());
                        break;
                    case GroupToken<Brace, Close>: return true;
                    default: throw new UnexpectedTokenException(token);
                }
            }

            return Completed;
        }
    }
}
