using System.Drawing;

namespace Paral.Lexing.Tokens.Blocks
{
    public class ParenthesisToken : BlockToken
    {
        public ParenthesisToken(Point location, BlockTokenIntent intent) : base(location, intent) { }
    }
}
