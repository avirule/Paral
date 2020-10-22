#region

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices.WindowsRuntime;
using Paral.Lexing.Tokens;
using Paral.Lexing.Tokens.Keywords;

#endregion


namespace Paral.Parsing.Nodes
{
    public class NamespaceNode : Node
    {
        public IdentifierToken Identifier { get; }

        public NamespaceNode(IdentifierToken identifier) => Identifier = identifier;

        protected override bool ConsumeTokenInternal(Token token)
        {
            if ((Leaves.Count > 0) && !Leaves[^1].Completed) Leaves[^1].ConsumeToken(token);
            else
            {
                switch (token)
                {
                    case RequiresToken:
                        Leaves.Add(new RequiresNode());
                        return false;
                    case IdentifierToken identifierToken:
                        Leaves.Add(new ValueNode(identifierToken));
                        return false;
                }
            }

            return false;
        }

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
