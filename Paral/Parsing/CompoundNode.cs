using System.Collections.Generic;

namespace Paral.Parsing
{
    public class CompoundNode : Node
    {
        public Queue<Node> ChildNodes { get; }

        public CompoundNode(params Node[] childNodes) => ChildNodes = new Queue<Node>(childNodes);
    }
}
