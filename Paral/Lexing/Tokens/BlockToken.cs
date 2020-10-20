#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public enum BlockTypes
    {
        ParenthesisOpen,
        ParenthesisClose,
        BracketOpen,
        BracketClose,
        SquareBracketOpen,
        SquareBracketClose
    }

    public class BlockToken : Token
    {
        public BlockTypes Type { get; }

        public BlockToken(Point location, BlockTypes type) : base(location) => Type = type;
    }
}
