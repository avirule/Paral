using System.Drawing;

namespace Paral.Compiler.Lexing.Tokens
{
    public enum Mutability
    {
        Immutable,
        Mutable
    }

    public class MutabilityToken : Token
    {
        public Mutability Mutability { get; }

        public MutabilityToken(Point location, Mutability mutability) : base(location) => Mutability = mutability;
    }
}
