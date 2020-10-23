using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public abstract class LeafNode : Node
    {
        protected LeafNode() => Completed = true;

        protected override bool ConsumeTokenInternal(Token token) => true;
    }
}
