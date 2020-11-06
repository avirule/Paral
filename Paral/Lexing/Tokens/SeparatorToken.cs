#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public interface ISeparator { }

    public class Path : ISeparator { }

    public class Member : ISeparator { }

    public class Comma : ISeparator { }

    public class SeparatorToken<TSeparator> : Token where TSeparator : ISeparator
    {
        public SeparatorToken(Point location) : base(location) { }
    }
}
