#region

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Paral.Exceptions;
using Paral.Lexing;
using Paral.Lexing.Tokens;
using Paral.Parsing.Nodes;

#endregion


namespace Paral.Parsing
{
    public class Parser
    {
        private readonly Lexer _Lexer;

        public Parser(Stream data) => _Lexer = new Lexer(data);

        public async Task<MasterNode> Parse()
        {
            MasterNode masterNode = new MasterNode();
            List<Token>? namespaceDeclaration = default;

            await foreach (Token token in _Lexer.Tokenize())
            {
                if (namespaceDeclaration is not null)
                {
                    switch (token)
                    {
                        case IdentifierToken identifierToken:
                            if (namespaceDeclaration.Count == 0) namespaceDeclaration.Add(identifierToken);
                            else if (namespaceDeclaration[^1] is AccessOperatorToken) namespaceDeclaration.Add(identifierToken);
                            else ThrowHelper.Throw(identifierToken, "Expected terminator or namespace access operator.");

                            break;
                        case AccessOperatorToken accessOperatorToken:
                            if ((namespaceDeclaration.Count > 0) && namespaceDeclaration[^1] is IdentifierToken)
                                namespaceDeclaration.Add(accessOperatorToken);
                            else ThrowHelper.ThrowExpectedIdentifier(accessOperatorToken);

                            break;
                        case TerminatorToken terminatorToken:
                            if ((namespaceDeclaration.Count > 0) && namespaceDeclaration[^1] is IdentifierToken)
                            {
                                namespaceDeclaration.Add(terminatorToken);
                                masterNode.SetCurrentNamespace(ConvertNamespaceDeclarationToIdentifiers(namespaceDeclaration));
                                namespaceDeclaration = default;
                            }
                            else ThrowHelper.Throw(terminatorToken, "Unexpected terminator.");

                            break;
                        default:
                            ThrowHelper.ThrowUnexpectedToken(token);
                            break;
                    }
                }
                else if (token is NamespaceToken)
                {
                    namespaceDeclaration = new List<Token>();
                }
                else masterNode.ConsumeToken(token);
            }

            return masterNode;
        }

        private static Stack<IdentifierToken> ConvertNamespaceDeclarationToIdentifiers(IEnumerable<Token> namespaceDeclaration) =>
            new Stack<IdentifierToken>(namespaceDeclaration.Where(token => token is IdentifierToken).Reverse().Cast<IdentifierToken>());
    }
}
