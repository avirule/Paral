#region

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public abstract class Node
    {
        public bool Completed { get; private set; }
        public List<Node> Leaves { get; }

        public Node() => Leaves = new List<Node>();

        public bool ConsumeToken(Token token) => Completed = ConsumeTokenInternal(token);

        protected abstract bool ConsumeTokenInternal(Token token);

        protected NamespaceNode AllocateNamespaceNode(IdentifierToken identifier)
        {
            NamespaceNode namespaceNode = new NamespaceNode(identifier);
            Leaves.Add(namespaceNode);
            return namespaceNode;
        }

        protected bool FindNamespaceNode(IdentifierToken identifier, [NotNullWhen(true)] out NamespaceNode? namespaceNode)
        {
            namespaceNode = Leaves.FirstOrDefault(node => node is NamespaceNode nsNode && nsNode.Identifier.Equals(identifier)) as NamespaceNode;
            return namespaceNode is not null;
        }

        public static IEnumerable<IdentifierToken> ParseIdentifiersFromNamespaceDeclaration(IEnumerable<Token> namespaceDeclaration) =>
            namespaceDeclaration.Where(token => token is IdentifierToken).Cast<IdentifierToken>();
    }
}
