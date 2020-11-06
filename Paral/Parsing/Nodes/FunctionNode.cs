using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class FunctionNode : Node
    {
        public string? Identifier { get; private set; }
        public string? ReturnType { get; private set; } // todo type check
        public ParametersNode? Parameters { get; private set; }
        public BlockNode? Body { get; private set; }

        protected override bool ConsumeTokenInternal(Token token)
        {
            if (ReturnType is null)
            {
                Expect<IdentifierToken>(token);
                ReturnType = (token as IdentifierToken)!.Value;
            }
            else if (Identifier is null)
            {
                Expect<IdentifierToken>(token);
                Identifier = (token as IdentifierToken)!.Value;
            }
            else if (Parameters is null)
            {
                Expect<GroupToken<Parenthetic, Open>>(token);
                Parameters = new ParametersNode();
            }
            else if (!Parameters.Completed) Parameters.ConsumeToken(token);
            else if (Body is null)
            {
                Expect<GroupToken<Brace, Open>>(token);
                Body = new BlockNode();
            }
            else if (!Body.Completed) return Body.Completed;

            return Completed;
        }
    }
}
