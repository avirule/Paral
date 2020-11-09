#region

using System.Drawing;

#endregion


namespace Paral.Compiler.Lexing.Tokens
{
    public abstract class Token
    {
        public Point Location { get; }

        protected Token(Point location) => Location = location;
    }
}
