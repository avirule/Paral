#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public interface IOperator { }

    public class NamespaceAccessor : IOperator { }

    public class Assignment : IOperator { }

    public class OperatorToken<T> : Token where T : IOperator
    {
        public OperatorToken(Point location) : base(location) { }
    }
}
