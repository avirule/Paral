using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class FunctionNode : Node
    {
        public string? Identifier { get; }

        // todo parameters
        public BlockNode? Body { get; }

        protected override bool ConsumeTokenInternal(Token token) => false;
    }
}
