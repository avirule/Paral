using Paral.Lexing.Tokens;

namespace Paral.Parsing.Nodes
{
    public class LiteralNode<T> : LeafNode where T : ILiteral
    {
        protected override bool ConsumeTokenInternal(Token token) => throw new System.NotImplementedException();
    }
}
