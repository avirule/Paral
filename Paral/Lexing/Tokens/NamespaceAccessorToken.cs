#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public class NamespaceAccessorToken : Token
    {
        public NamespaceAccessorToken(Point location) : base(location) { }
    }
}
