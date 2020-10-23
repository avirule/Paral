#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public interface IKeyword { }

    public class Requires : IKeyword { }

    public class Namespace : IKeyword { }

    public class Struct : IKeyword { }

    public class Implements : IKeyword { }

    public class KeywordToken<T> : Token where T : IKeyword
    {
        public KeywordToken(Point location) : base(location) { }
    }
}
