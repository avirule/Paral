using System.Collections.Generic;
using Paral.Exceptions;
using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class NamespaceNode : Node
    {
        public List<Token> DescriptionTokens { get; }

        public NamespaceNode() => DescriptionTokens = new List<Token>();

        protected override bool ConsumeTokenInternal(Token token)
        {
            switch (token)
            {
                // valid cases
                case IdentifierToken identifierToken when (DescriptionTokens.Count == 0) || DescriptionTokens[^1] is AccessOperatorToken:
                    DescriptionTokens.Add(identifierToken);
                    return false;
                case AccessOperatorToken accessOperatorToken when DescriptionTokens[^1] is IdentifierToken:
                    DescriptionTokens.Add(accessOperatorToken);
                    return false;
                case TerminatorToken _ when DescriptionTokens[^1] is IdentifierToken: return true;
                // error cases
                case IdentifierToken _:
                    ThrowHelper.Throw(token, "Expected terminator or namespace accessor symbol.");
                    return false;
                case AccessOperatorToken _:
                    ThrowHelper.ThrowExpectedIdentifier(token);
                    return false;
                case TerminatorToken _:
                    ThrowHelper.Throw("Unexpected terminator.");
                    return false;
                default:
                    ThrowHelper.ThrowUnexpectedToken(token);
                    return false;
            }
        }
    }
}
