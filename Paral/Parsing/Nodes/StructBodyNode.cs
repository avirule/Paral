using Paral.Lexing;
using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class StructBodyNode : BranchNode
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
                case MutabilityToken<Mutable>:
                    Branches.Add(new VariableNode<Mutable>());
                    break;
                case MutabilityToken<Immutable>:
                    Branches.Add(new VariableNode<Immutable>());
                    break;
                default: throw new UnexpectedTokenException(token);
            }

            return Completed;
        }
    }
}
