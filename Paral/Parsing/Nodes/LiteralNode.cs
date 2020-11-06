using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class LiteralNode : Node
    {
        public Literal Type { get; }
        public string Value { get; }

        public LiteralNode(Literal type, string value) => (Type, Value) = (type, value);

        protected override bool ConsumeTokenInternal(Token token) => true;
    }
}
