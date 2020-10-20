#region

using System.Collections.Generic;
using System.Linq;
using Paral.Exceptions;
using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class NamespaceNode : Node
    {
        private readonly List<Token> _IdentifierTokens;

        public string Identifier { get; private set; }

        public NamespaceNode(string identifier)
        {
            _IdentifierTokens = new List<Token>();
            Identifier = identifier;
        }

        public override void ConsumeToken(Token token)
        {
            if (string.IsNullOrEmpty(Identifier))
            {
                // parse as namespace identifier
                switch (token)
                {
                    case IdentifierToken identifierToken:
                        if (_IdentifierTokens.Count == 0) _IdentifierTokens.Add(identifierToken);
                        else if (_IdentifierTokens[^1] is AccessOperatorToken) _IdentifierTokens.Add(identifierToken);
                        else ThrowHelper.Throw(identifierToken, "Expected terminator or namespace access operator.");

                        break;
                    case AccessOperatorToken accessOperatorToken:
                        if ((_IdentifierTokens.Count > 0) && _IdentifierTokens[^1] is IdentifierToken) _IdentifierTokens.Add(accessOperatorToken);
                        else ThrowHelper.ThrowExpectedIdentifier(accessOperatorToken);

                        break;
                    case TerminatorToken terminatorToken:
                        if ((_IdentifierTokens.Count > 0) && _IdentifierTokens[^1] is IdentifierToken)
                        {
                            _IdentifierTokens.Add(terminatorToken);
                            Identifier = string.Join("::", _IdentifierTokens.Where(_token => _token is IdentifierToken));
                        }
                        else ThrowHelper.Throw(terminatorToken, "Unexpected terminator.");

                        break;
                    default:
                        ThrowHelper.ThrowUnexpectedToken(token);
                        break;
                }
            }
        }
    }
}
