#region

using System.Collections.Generic;
using Paral.Lexing;

#endregion

namespace Paral.Parsing.Nodes
{
    public class LogicalNode : Node
    {
        private bool _ArgumentsTerminated;

        public List<ArgumentNode> Arguments { get; }
        public Node? LogicChain { get; private set; }

        public LogicalNode()
        {
            Arguments = new List<ArgumentNode>();
            LogicChain = null;
        }

        protected override bool ConsumeTokenInternal(Token token)
        {
            switch (token.Type)
            {
                case TokenType.CurlyBracketOpen when !_ArgumentsTerminated:
                    _ArgumentsTerminated = true;
                    return false;
                case TokenType.Identifier when !_ArgumentsTerminated && (Arguments.Count == 0):
                    Arguments.Add(new ArgumentNode());
                    Arguments[^1].ConsumeToken(token);
                    return false;
                case TokenType.Identifier when !_ArgumentsTerminated && !Arguments[^1].Complete:
                    Arguments[^1].ConsumeToken(token);
                    return false;
                case TokenType.ArgumentSeparator when !_ArgumentsTerminated && (Arguments.Count > 0) && Arguments[^1].Complete:
                    Arguments.Add(new ArgumentNode());
                    return false;
                case TokenType.Identifier when !_ArgumentsTerminated && (Arguments.Count > 0):
                    Arguments[^1].ConsumeToken(token);
                    return false;
                default:
                    LogicChain?.ConsumeToken(token);
                    return false;
            }
        }
    }
}
