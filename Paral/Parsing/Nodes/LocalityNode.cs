#region

using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class LocalityNode : Node
    {
        public LocalityIdentifier Locality { get; }
        public Node Child { get; }

        public LocalityNode(LocalityIdentifier locality) => Locality = locality;

        public override void Consume(Token token)
        {

        }
    }
}
