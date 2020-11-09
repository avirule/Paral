using Paral.Compiler.Lexing.Tokens;
using Paral.Compiler.Parsing.Nodes.Functions;

namespace Paral.Compiler.Parsing.Nodes
{
    public class VariableNode : Node
    {
        public Mutability Mutability { get; }
        public string? RuntimeType { get; private set; } // todo type check
        public string? Identifier { get; private set; }
        public ExpressionNode? Value { get; private set; }

        public VariableNode(Mutability mutability) => Mutability = mutability;

        protected override bool ConsumeTokenInternal(Token token)
        {
            if (RuntimeType is null)
            {
                Expect<IdentifierToken>(token);
                RuntimeType = (token as IdentifierToken)!.Value;
            }
            else if (Identifier is null)
            {
                Expect<IdentifierToken>(token);
                RuntimeType = (token as IdentifierToken)!.Value;
            }
            else if (Value is null) return (Value = new ExpressionNode()).ConsumeToken(token);
            else if (!Value.Completed) return Value.ConsumeToken(token);

            return Completed;
        }
    }
}
