using System;

namespace Paral.Compiler.Parsing
{
    public struct RuntimeType : IEquatable<RuntimeType>
    {
        public Scope Scope { get; }
        public string Name { get; }

        public RuntimeType(Scope scope, string name) => (Scope, Name) = (scope, name);

        public bool Equals(RuntimeType other) => Scope.Equals(other.Scope) && Name.Equals(other.Name, StringComparison.InvariantCulture);
        public override bool Equals(object? obj) => obj is RuntimeType other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Scope, Name);

        public static bool operator ==(RuntimeType left, RuntimeType right) => left.Equals(right);
        public static bool operator !=(RuntimeType left, RuntimeType right) => !left.Equals(right);
    }
}
