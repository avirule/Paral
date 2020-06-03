#region

using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public abstract class Node
    {
        public bool Complete { get; private set; }

        protected abstract bool ConsumeTokenInternal(Token token);

        public bool ConsumeToken(Token token) => Complete = ConsumeTokenInternal(token);
    }
}
