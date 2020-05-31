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
        private static readonly HashSet<string> LocalityIdentifiers = new HashSet<string>
        {
            "public", "private"
        };

        private readonly Lexer _Lexer;

        private Queue<Token> _Tokens;

        public CompoundNode RootNode { get; }

        public Parser(Lexer lexer)
        {
            _Lexer = lexer;

            RootNode = new CompoundNode();
        }

        public void Parse()
        {




            return;

            //_Tokens = new Queue<Token>(_Lexer.Tokenize());

            while (TryEvaluateNextNode(out Node node))
            {
                RootNode.ChildNodes.Enqueue(node);
            }
        }

        private bool TryEvaluateNextNode(out Node node)
        {
            node = default;

            if (!_Tokens.TryDequeue(out Token token))
            {
                return false;
            }

            switch (token.Type)
            {
                case TokenType.Identifier:
                {
                    if (LocalityIdentifiers.Contains(token.Value))
                    {
                        if (!_Tokens.TryDequeue(out Token typeToken) || typeToken.Type != TokenType.Identifier)
                        {
                            ExceptionHelper.Error(token.Location, $"Locality identifier must be followed by type identifier.");
                            return false;
                        }
                        else if (!_Tokens.TryDequeue(out Token nameToken) || nameToken.Type != TokenType.Identifier)
                        {
                            ExceptionHelper.Error(typeToken.Location, $"Type identifier must be followed by function name.");
                            return false;
                        }
                    } else if (/* todo process variable declaration */ false ) {}
                    else
                    {
                        ExceptionHelper.Error(token.Location, $"Invalid identifier \"{token.Value}\" encountered.");
                    }

                    break;
                }
                case TokenType.Operator:
                    break;
                case TokenType.CurlyBracketOpen:
                    break;
                case TokenType.ParenthesisOpen:
                {
                    if ( // check for empty control flow (i.e. '()'  )
                        (_Tokens.TryPeek(out Token nextToken)
                         && (nextToken.Type == TokenType.ParenthesisClose)
                         && _Tokens.TryDequeue(out nextToken))
                        // general catch for failed evaluation
                        // remark: error should've been thrown within the function
                        || !TryEvaluateNextNode(out node))
                    {
                        // empty control flow statement (i.e. --> () <-- )
                        return false;
                    }
                    else if (!_Tokens.TryDequeue(out nextToken) || (nextToken.Type != TokenType.ParenthesisClose))
                    {
                        ExceptionHelper.Error(token.Location, "Opening parenthesis has no complement closure (missing a ')' somewhere).");
                        return false;
                    }

                    return true;
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
                case TokenType.DecimalLiteral:
                {
                    if (!_Tokens.TryPeek(out Token nextToken))
                    {
                        ExceptionHelper.Error(token.Location, "Numeric literal must have postceding token.");
                        return false;
                    }

                    switch (nextToken.Type)
                    {
                        case TokenType.Operator when _Tokens.TryDequeue(out nextToken):
                            if (TryEvaluateNextNode(out Node nextNode))
                            {
                                node = new CompoundNode(new Node(token), new Node(nextToken), nextNode);
                                return true;
                            }

                            return false;
                        case TokenType.ParenthesisClose: // if it's a closure paren, it should be consumed further up the stack
                        case TokenType.StatementClosure when _Tokens.TryDequeue(out nextToken): // consume closing semicolon
                            node = new Node(token);
                            return true;
                        default:
                            Debug.Assert(nextToken != null, "Token should have been checked notnull in preceding control flow.");

                            ExceptionHelper.Error(token.Location, $"Invalid postceding token type: {nextToken.Type}.");
                            return false;
                    }
                }
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

            ExceptionHelper.Error(token.Location, "Parser error.");
            return false;
        }
    }
}
