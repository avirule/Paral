#region

using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public abstract class Node
    {
        public bool Complete { get; protected set; }

        public abstract void Consume(Token token);
    }
}
