#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens.Blocks
{
    public class BracketToken : BlockToken
    {
        public BracketToken(Point location, BlockTokenIntent intent) : base(location, intent) { }
    }
}
