#region

using System;
using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class LiteralNode<T> : LeafNode where T : ILiteral
    {
        protected override bool ConsumeTokenInternal(Token token) => throw new NotImplementedException();
    }
}
