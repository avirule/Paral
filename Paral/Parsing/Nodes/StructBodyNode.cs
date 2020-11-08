using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class StructBodyNode : BranchNode
    {
        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Branches.Count > 0) && !Branches[^1].Completed)
            {
                Branches[^1].ConsumeToken(token);
                return Completed;
            }

            if (_Tokens.Count == 0)
            {
                switch (token)
                {
                    case MutabilityToken<Mutable>:
                    case MutabilityToken<Immutable>: break;
                }
            }
            else
                switch (_Tokens[^1]) { }

            return Completed;
        }
    }

    public class VariableNode<TMutability> : Node where TMutability : IMutability
    {

        public string? RuntimeType { get; private set; } // todo type check
        public

        protected override bool ConsumeTokenInternal(Token token) { }
    }
}
