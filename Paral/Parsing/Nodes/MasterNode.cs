#region

using System.Collections.Generic;
using Paral.Exceptions;
using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class MasterNode : Node
    {
        private NamespaceNode? _CurrentNamespace;

        public override void ConsumeToken(Token token)
        {
            if (_CurrentNamespace is null) ThrowHelper.Throw(token, "Namespace has not been declared for scope.");
            else _CurrentNamespace.ConsumeToken(token);
        }

        public void SetCurrentNamespace(Stack<IdentifierToken> identifiers)
        {
            IdentifierToken identifier = identifiers.Pop();

            if (FindNamespaceNode(identifier, out _CurrentNamespace))
                _CurrentNamespace.TryGetNamespaceNodeRecursive(identifiers, out _CurrentNamespace);
            else AllocateNamespaceNode(identifier).TryGetNamespaceNodeRecursive(identifiers, out _CurrentNamespace);
        }
    }
}
