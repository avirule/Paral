#region

using System.Collections.Generic;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class CompoundArgumentsNode : Node
    {
        public List<ArgumentNode> Arguments { get; }

        public CompoundArgumentsNode() => Arguments = new List<ArgumentNode>();

        public override void Consume(Token token)
        {
            if ((token.Type == TokenType.ParenthesisClose) && ((Arguments.Count == 0) || Arguments[^1].Complete))
            {
                Complete = true;
            }
            else
            {
                if (Arguments.Count == 0)
                {
                    Arguments.Add(new ArgumentNode());
                }

                Arguments[^1].Consume(token);
            }
        }
    }
}
