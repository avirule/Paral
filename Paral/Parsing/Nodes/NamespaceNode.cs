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
            if ((Branches.Count > 0) && !Branches[^1].Completed) Branches[^1].ConsumeToken(token);
            else
            {
                switch (token)
                {
                    case RequiresToken:
                        Branches.Add(new RequiresNode());
                        break;
                    case IdentifierToken identifierToken:
                        Branches.Add(new FunctionNode(identifierToken));
                        break;
                    case StructToken _:
                        Branches.Add(new StructNode());
                        break;
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
