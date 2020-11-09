using Paral.Compiler.Lexing;
using Paral.Compiler.Lexing.Tokens;

namespace Paral.Compiler.Parsing.Nodes.Functions
{
    public class FunctionBodyNode : BranchNode
    {
        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed)
            {
                Branches[^1].ConsumeToken(token);

                return Completed;
            }

            switch (token)
                {
                    case KeywordToken<Return>:
                        Branches.Add(new ReturnExpressionNode());
                        break;
                    case GroupToken<Brace, Close>: return true;
                    default: throw new UnexpectedTokenException(token);
                }

                return Completed;
        }
    }
}
