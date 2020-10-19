#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public enum AccessOperator
    {
        Namespace,
        Member
    }

    public class AccessOperatorToken : Token
    {
        public AccessOperator Value { get; }

        public AccessOperatorToken(Point location, AccessOperator value) : base(location) => Value = value;
    }
}
