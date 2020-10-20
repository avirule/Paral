#region

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Paral.Lexing;
using Paral.Lexing.Tokens;

#endregion

namespace Paral.Parsing.Nodes
{
    public abstract class Node
    {
        public List<Node> Leaves { get; }

        public Node() => Leaves = new List<Node>();

        public abstract void ConsumeToken(Token token);

        protected bool FindNamespaceNode(string namespaceIdentifier, [NotNullWhen(true)] out NamespaceNode? namespaceNode)
        {
            foreach (Node node in Leaves.Where(node => node is NamespaceNode))
            {
                namespaceNode = node as NamespaceNode;

                // we've verified given node is of type NamespaceNode, so cast is
                // valid and variable isn't null
                if (namespaceNode!.Identifier.Equals(namespaceIdentifier))
                {
                    return true;
                }
            }

            namespaceNode = default;
            return false;
        }
    }
}
