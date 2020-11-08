using System.Drawing;

namespace Paral.Lexing.Tokens
{
    public interface IMutability { }

    public class Mutable : IMutability { }

    public class Immutable : IMutability { }

    public class MutabilityToken<TMutability> : Token where TMutability : IMutability
    {
        public MutabilityToken(Point location) : base(location) { }
    }
}
