#region

using System.Drawing;

#endregion

namespace Paral.Lexing
{
    public enum TokenType
    {
        Identifier,
        Operator,
        CharacterLiteral,
        StringLiteral,
        NumericLiteral,
        DecimalLiteral,
        CurlyBracketOpen,
        CurlyBracketClose,
        ParenthesisOpen,
        ParenthesisClose,
        StatementClosure,
        StatementConcat,
        ArgumentSeparator,
        EndOfFile
    }

    public class Token
    {
        public Point Location { get; }
        public TokenType Type { get; }
        public string Value { get; }

        public Token(Point location, TokenType tokenType, string value) => (Location, Type, Value) = (location, tokenType, value);
    }
}
