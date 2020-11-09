using System;

namespace Paral.Compiler
{
    public readonly struct Scope : IEquatable<Scope>
    {
        public Guid Identity { get; }

        public Scope(Guid identity) => Identity = identity;

        public bool Equals(Scope other) => Identity.Equals(other.Identity);
        public override bool Equals(object? obj) => obj is Scope other && Equals(other);

        public override int GetHashCode() => Identity.GetHashCode();

        public static bool operator ==(Scope left, Scope right) => left.Equals(right);
        public static bool operator !=(Scope left, Scope right) => !left.Equals(right);
    }
}
