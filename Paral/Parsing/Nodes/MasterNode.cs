#region

using System.Collections.Generic;
using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class MasterNode : Node
    {
        private NamespaceNode? _CurrentNamespace;

        public override void ConsumeToken(Token token) { }

        public void SetCurrentNamespace(Stack<IdentifierToken> identifiers)
        {
            IdentifierToken identifier = identifiers.Pop();

            if (FindNamespaceNode(identifier, out _CurrentNamespace))
                _CurrentNamespace.TryGetNamespaceNodeRecursive(identifiers, out _CurrentNamespace);
            else AllocateNamespaceNode(identifier).TryGetNamespaceNodeRecursive(identifiers, out _CurrentNamespace);
        }
    }
}
