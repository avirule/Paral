#region

using System.Drawing;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public abstract class Node
    {
        public Point Location { get; protected set; }
        public bool Complete { get; protected set; }

        public abstract void Consume(Token token);
    }
}
