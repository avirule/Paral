using Paral.Lexing;

namespace Paral.Parsing.Nodes
{
    public class IdentifierNode : BranchNode
    {
        public string Identifier { get; private set; }

        public IdentifierNode(string identifier) => Identifier = identifier;

        protected override void AttemptInitializeChild(Token token)
        {
            // todo implement this
        }
    }
}
