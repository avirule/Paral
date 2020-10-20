#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public class EOFToken : Token
    {
        public EOFToken(Point location) : base(location) { }
    }
}
