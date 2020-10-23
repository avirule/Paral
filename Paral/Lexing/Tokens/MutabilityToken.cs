#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public interface IMutability { }

    public class Mutable : IMutability { }

    public class Immutable : IMutability { }

    public class Constant : IMutability { }

    public class MutabilityToken<T> : Token where T : IMutability
    {
        public MutabilityToken(Point location) : base(location) { }
    }
}
