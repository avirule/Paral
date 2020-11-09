#region

using System.Drawing;

#endregion


namespace Paral.Compiler.Lexing.Tokens
{
    public class EOFToken : Token
    {
        public EOFToken(Point location) : base(location) { }
    }
}
