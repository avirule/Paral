#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public interface IKeyword { }

    public class Requires : IKeyword { }

    public class Namespace : IKeyword { }

    public class Struct : IKeyword { }

    public class Function : IKeyword { }

    public class Implements : IKeyword { }

    public class Return : IKeyword { }

    public class Immutable : IKeyword { }

    public class Mutable : IKeyword { }

    public class KeywordToken<T> : Token where T : IKeyword
    {
        public KeywordToken(Point location) : base(location) { }
    }
}
