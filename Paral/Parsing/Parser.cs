#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Paral.Exceptions;
using Paral.Lexing;

#endregion

namespace Paral.Parsing
{
    public class Parser
    {
        private readonly Queue<Token> _Tokens;

        public CompoundNode RootNode { get; }

        public Parser(Lexer lexer)
        {
            _Tokens = new Queue<Token>(lexer.Tokens);
            RootNode = new CompoundNode();
        }

        public void Parse()
        {
            while (TryEvaluateNextNode(out Node node))
            {
                RootNode.ChildNodes.Enqueue(node);
            }
        }

        private bool TryEvaluateNextNode(out Node node)
        {
            node = default;

            if (_Tokens.TryDequeue(out Token token))
            {
                switch (token.TokenType)
                {
                    case TokenType.Identifier:
                    {
                        if (!_Tokens.TryDequeue(out Token nextToken))
                        {
                            ExceptionHelper.Error(token.Location, "Identifier must have postceding token.");
                            return false;
                        }

                        switch (nextToken.TokenType) { }

                        break;
                    }
                    case TokenType.Operator:
                        break;
                    case TokenType.CurlyBracketOpen:
                        break;
                    case TokenType.ParenthesisOpen:
                    {
                        if (_Tokens.TryPeek(out Token nextToken)
                            && (nextToken.TokenType == TokenType.ParenthesisClose)
                            && _Tokens.TryDequeue(out nextToken))
                        {
                            // empty control flow statement (i.e. --> () <-- )
                            return false;
                        }
                        else if (TryEvaluateNextNode(out node))
                        {
                            if (!_Tokens.TryDequeue(out nextToken) || (nextToken.TokenType != TokenType.ParenthesisClose))
                            {
                                ExceptionHelper.Error(token.Location, "Opening parenthesis has no complement closure (missing a ')' somewhere).");
                                return false;
                            }

                            return true;
                        }

                        break;
                    }
                    case TokenType.CurlyBracketClose:
                    case TokenType.ParenthesisClose:
                        ExceptionHelper.Error(token.Location, "Invalid hanging parenthesis closure (missing a '(' somewhere).");
                        break;
                    case TokenType.CharacterLiteral:
                        break;
                    case TokenType.StringLiteral:
                        break;
                    case TokenType.NumericLiteral:
                    {
                        if (!_Tokens.TryPeek(out Token nextToken))
                        {
                            ExceptionHelper.Error(token.Location, "Numeric literal must have postceding token.");
                            return false;
                        }

                        switch (nextToken.TokenType)
                        {
                            case TokenType.Operator when _Tokens.TryDequeue(out nextToken):
                                if (TryEvaluateNextNode(out Node nextNode))
                                {
                                    node = new CompoundNode(new Node(token), new Node(nextToken), nextNode);
                                    return true;
                                }

                                return false;
                            case TokenType.ParenthesisClose:
                            case TokenType.StatementClosure when _Tokens.TryDequeue(out nextToken):
                                node = new Node(token);
                                return true;
                            default:
                                Debug.Assert(nextToken != null, "Token should have been checked in preceding control flow.");

                                ExceptionHelper.Error(token.Location, $"Invalid postceding token type: {nextToken.TokenType}.");
                                return false;
                        }
                    }
                    case TokenType.DecimalLiteral:
                        break;
                    case TokenType.StatementClosure:
                        break;
                    case TokenType.StatementConcat:
                        break;
                    case TokenType.ArgumentSeparator:
                        break;
                    case TokenType.EndOfFile:
                        return false;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return false;
        }
    }
}
