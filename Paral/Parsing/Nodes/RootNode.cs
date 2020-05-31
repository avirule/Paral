#region

using System.Collections.Generic;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class RootNode : Node
    {
        private readonly List<Node> _Children;

        public RootNode() => _Children = new List<Node>();

        public override void Consume(Token token)
        {
            if (_Children.Count == 0)
            {
                // todo this is testing
                _Children.Add(new FunctionNode());
            }

            if (!_Children[^1].Complete)
            {
                _Children[^1].Consume(token);
            }
        }
    }
}
