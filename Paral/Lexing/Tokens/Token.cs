#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public abstract class Token
    {
        public Point Location { get; }

        protected Token(Point location) => Location = location;
    }
}
