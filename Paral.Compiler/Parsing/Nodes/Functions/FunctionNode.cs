using Paral.Compiler.Lexing.Tokens;

namespace Paral.Compiler.Parsing.Nodes.Functions
{
    public class FunctionNode : Node
    {
        public string? Identifier { get; private set; }
        public string? ReturnType { get; private set; } // todo type check
        public ParametersNode? Parameters { get; private set; }
        public FunctionBodyNode? Body { get; private set; }

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
                Expect<GroupToken<Paren, Open>>(token);
                Parameters = new ParametersNode();
            }
            else if (!Parameters.Completed) Parameters.ConsumeToken(token);
            else if (Body is null)
            {
                Expect<GroupToken<Brace, Open>>(token);
                Body = new FunctionBodyNode();
            }
            else if (!Body.Completed) return Body.ConsumeToken(token);

            return Completed;
        }
    }
}
