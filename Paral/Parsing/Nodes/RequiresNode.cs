#region

using System.Collections.Generic;
using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class RequiresNode : Node
    {
        private readonly List<Token> _NamespaceDeclaration;

        public IEnumerable<IdentifierToken> IdentifierTokens => ParseIdentifiersFromNamespaceDeclaration(_NamespaceDeclaration);

        public RequiresNode() => _NamespaceDeclaration = new List<Token>();

        protected override bool ConsumeTokenInternal(Token token)
        {
            switch (token)
            {
                case IdentifierToken identifierToken:
                    if (_NamespaceDeclaration.Count == 0) _NamespaceDeclaration.Add(identifierToken);
                    else if (_NamespaceDeclaration[^1] is OperatorToken<NamespaceAccessor>) _NamespaceDeclaration.Add(identifierToken);
                    else ThrowHelper.Throw(identifierToken, "Expected terminator or namespace access operator.");

                    return false;
                case OperatorToken<NamespaceAccessor> namespaceAccessorToken:
                    if ((_NamespaceDeclaration.Count > 0) && _NamespaceDeclaration[^1] is IdentifierToken) _NamespaceDeclaration.Add(namespaceAccessorToken);
                    else ThrowHelper.ThrowExpectedIdentifier(namespaceAccessorToken);

                    return false;
                case TerminatorToken terminatorToken:
                    if ((_NamespaceDeclaration.Count > 0) && _NamespaceDeclaration[^1] is IdentifierToken) return true;
                    else ThrowHelper.Throw(terminatorToken, "Unexpected terminator.");

                    return true;
                default:
                    ThrowHelper.ThrowUnexpectedToken(token);
                    return false;
            }
        }
    }
}
