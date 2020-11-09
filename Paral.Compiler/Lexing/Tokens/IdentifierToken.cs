#region

using System;
using System.Drawing;

#endregion


namespace Paral.Compiler.Lexing.Tokens
{
    public class IdentifierToken : Token, IEquatable<IdentifierToken>
    {
        public string Value { get; }

        public IdentifierToken(Point location, string value) : base(location) => Value = value;

        public bool Equals(IdentifierToken? other) => Value.Equals(other?.Value);
        public override bool Equals(object? obj) => obj is IdentifierToken identifierToken && Equals(identifierToken);

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(IdentifierToken? left, IdentifierToken? right) => Equals(left, right);
        public static bool operator !=(IdentifierToken? left, IdentifierToken? right) => !Equals(left, right);
    }
}
