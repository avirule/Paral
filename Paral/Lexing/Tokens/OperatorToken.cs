#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public interface IOperatorTokenType { }

    public class Add : IOperatorTokenType { }

    public class Subtract : IOperatorTokenType { }

    public class Multiply : IOperatorTokenType { }

    public class Divide : IOperatorTokenType { }

    public class NamespaceAccessor : IOperatorTokenType { }

    public class RuntimeType : IOperatorTokenType { }

    public class Assignment : IOperatorTokenType { }

    public class OperatorToken<T> : Token where T : IOperatorTokenType
    {
        public OperatorToken(Point location) : base(location) { }
    }
}
