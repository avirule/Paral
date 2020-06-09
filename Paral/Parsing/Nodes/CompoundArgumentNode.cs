#region

using System.Collections.Generic;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class CompoundArgumentNode : Node
    {
        public List<ArgumentNode> Arguments { get; }

        public CompoundArgumentNode() => Arguments = new List<ArgumentNode>();

        protected override bool ConsumeTokenInternal(Token token)
        {
            switch (token.Type)
            {
                case TokenType.Identifier when Arguments.Count == 0:
                    Arguments.Add(new ArgumentNode());
                    break;
                case TokenType.ArgumentSeparator when Arguments[^1].Complete:
                    Arguments.Add(new ArgumentNode());
                    return false; // don't pass separator to argument node
                case TokenType.ParenthesisClose when (Arguments.Count == 0) || Arguments[^1].Complete:
                    return true; // we've completed the compound argument
            }

            Arguments[^1].ConsumeToken(token);
            return false;
        }
    }
}
