#region

using System;
using Paral.Lexing.Tokens;

#endregion


namespace Paral.Parsing.Nodes
{
    public class ArithmeticNode : Node
    {
        public ArithmeticOperator Operator { get; }

        public ArithmeticNode(ArithmeticOperator @operator) => Operator = @operator;

        protected override bool ConsumeTokenInternal(Token token) => throw new NotImplementedException();
    }
}
