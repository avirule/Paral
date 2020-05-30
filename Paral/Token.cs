#region

using System.Drawing;

#endregion

namespace Paral
{
    public enum TokenType
    {
        Identifier,
        Operator,
        CharacterLiteral,
        StringLiteral,
        NumericLiteral,
        DecimalLiteral,
        ControlFlow,
        SingularFlow,
        NewLine,
        EndOfFile
    }

    public class Token
    {
        public Point Location { get; }
        public TokenType TokenType { get; }
        public string Value { get; }

        public Token(Point location, TokenType tokenType, string value) => (Location, TokenType, Value) = (location, tokenType, value);
    }
}
