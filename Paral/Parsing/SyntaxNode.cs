using Paral.Lexing;

namespace Paral.Parsing
{
    public class SyntaxNode : Node
    {
        public Token Token { get; }

        public SyntaxNode(Token token) => Token = Token;
    }
}
