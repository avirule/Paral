#region

using System.Drawing;

#endregion


namespace Paral.Lexing.Tokens
{
    public interface IOperator { }

    public class Assignment : IOperator { }

    public class Add : IOperator { }

    public class Subtract : IOperator { }

    public class Multiply : IOperator { }

    public class Divide : IOperator { }

    public class OperatorToken<T> : Token where T : IOperator
    {
        public OperatorToken(Point location) : base(location) { }
    }
}
