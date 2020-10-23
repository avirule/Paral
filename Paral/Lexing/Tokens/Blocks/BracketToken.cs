using System.Drawing;

namespace Paral.Lexing.Tokens.Blocks
{
    public class BracketToken : BlockToken
    {
        public BracketToken(Point location, BlockTokenIntent intent) : base(location, intent) { }
    }
}
