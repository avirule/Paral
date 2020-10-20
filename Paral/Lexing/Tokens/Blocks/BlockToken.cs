#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens.Blocks
{
    public enum BlockTokenIntent
    {
        Open,
        Close
    }

    public class BlockToken : Token
    {
        public BlockTokenIntent Intent { get; }

        public BlockToken(Point location, BlockTokenIntent intent) : base(location) => Intent = intent;
    }
}
