using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class VariableNode<TMutability> : Node where TMutability : IMutability
    {
        public string? RuntimeType { get; private set; } // todo type check
        public string? Identifier { get; private set; }
        public ExpressionNode? Value { get; private set; }

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
