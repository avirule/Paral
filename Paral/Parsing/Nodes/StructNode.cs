using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class StructNode : Node
    {
        public string? Identifier { get; private set; } // todo type check
        public BlockNode? Body { get; private set; }

        protected override bool ConsumeTokenInternal(Token token)
        {
            if (Identifier is null)
            {
                Expect<IdentifierToken>(token);

                Identifier = (token as IdentifierToken)!.Value;
            }
            else if (Body is null)
            {
                Expect<GroupToken<Brace, Open>>(token);

                Body = new BlockNode();
            }
            else if (!Body.Completed) return Body.ConsumeToken(token);

            return Completed;
        }
    }
}
