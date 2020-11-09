using Paral.Compiler.Lexing.Tokens;
using Paral.Compiler.Parsing.Nodes.Functions;

namespace Paral.Compiler.Parsing.Nodes
{
    public class NamespaceNode : BranchNode
    {
        public string Identity { get; }

        public NamespaceNode(string identity) => Identity = identity;

        public bool IsComplete() => (Branches.Count == 0) || Branches[^1].Completed;

        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed) Branches[^1].ConsumeToken(token);
            else if (_Tokens.Count > 0)
            {
                switch (_Tokens[^1])
                {
                    // todo handle tokens
                }
            }
            else
            {
                switch (token)
                {
                    case KeywordToken<Struct>:
                        Branches.Add(new StructNode());
                        break;
                    case KeywordToken<Function>:
                        Branches.Add(new FunctionNode());
                        break;
                }
            }

            return Completed;
        }
    }
}
