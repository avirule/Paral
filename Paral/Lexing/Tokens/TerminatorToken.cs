#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public class TerminatorToken : Token
    {
        public TerminatorToken(Point location) : base(location) { }
    }
}
