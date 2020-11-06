using System;
using Paral.Lexing.Tokens;

namespace Paral.Lexing
{
    public class ExpectedTokenException : Exception
    {
        public Type Expected { get; }
        public Type Actual { get; }

        public ExpectedTokenException(Type expected, Token actual) : base(FormatHelper.ExpectedToken(expected, actual)) =>
            (Expected, Actual) = (expected, actual.GetType());
    }

    public class UnexpectedTokenException : Exception
    {
        public Token Token { get; }
        public UnexpectedTokenException(Token token) : base(FormatHelper.UnexpectedToken(token)) => Token = token;
    }
}
