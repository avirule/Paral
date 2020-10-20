#region

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class NamespaceNode : Node
    {
        public IdentifierToken Identifier { get; }

        public NamespaceNode(IdentifierToken identifier) => Identifier = identifier;

        public override void ConsumeToken(Token token) { }

        public bool TryGetNamespaceNodeRecursive(Stack<IdentifierToken> identifiers, [NotNullWhen(true)] out NamespaceNode? namespaceNode)
        {
            if (identifiers.Count == 0)
            {
                namespaceNode = this;
                return true;
            }
            else if (identifiers.TryPop(out IdentifierToken? identifier))
            {
                if (FindNamespaceNode(identifier, out namespaceNode))
                    return namespaceNode.TryGetNamespaceNodeRecursive(identifiers, out namespaceNode);
                else return AllocateNamespaceNode(identifier).TryGetNamespaceNodeRecursive(identifiers, out namespaceNode);
            }

            namespaceNode = default;
            return false;
        }
    }
}
