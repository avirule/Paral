using System;

namespace Paral.Parsing
{
    public class FunctionNode : Node
    {
        public FunctionLocalityIdentifier Locality { get; }
        public RuntimeType Type { get; }
        public string Name { get; }
        public Node Expression { get; }

        public FunctionNode(FunctionLocalityIdentifier locality, RuntimeType runtimeType, Node expression)
        {
            Locality = locality;
            Type = runtimeType;
            Expression = expression;
        }
    }
}
