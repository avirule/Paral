#region

using Paral.Lexing;

#endregion

namespace Paral.Parsing
{
    public class Node
    {
        public Token Value { get; }

        public Node() { }
        public Node(Token value) => Value = value;
    }
}
