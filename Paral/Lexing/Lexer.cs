#region

using System;
using System.Collections.Generic;
using System.Drawing;
using Paral.Exceptions;

#endregion

namespace Paral.Lexing
{
    public class Lexer
    {
        private readonly char[] _Data;

        private int _StartIndex;
        private int _RunIndex;
        private Point _Location;

        public Lexer(char[] data)
        {
            _Data = data;
            _StartIndex = _RunIndex = 0;
            _Location = new Point(1, 1);
        }

        public IEnumerable<Token> Tokenize()
        {
            char character;
            while (!IsEndOfFile(character = Scan()))
            {
                // reset start index
                _StartIndex = _RunIndex;

                switch (character)
                {
                    case '(' when Advance():
                        yield return new Token(_Location, TokenType.ParenthesisOpen, character.ToString());
                        break;
                    case ')' when Advance():
                        yield return new Token(_Location, TokenType.ParenthesisClose, character.ToString());
                        break;
                    case '{' when Advance():
                        yield return new Token(_Location, TokenType.CurlyBracketOpen, character.ToString());
                        break;
                    case '}' when Advance():
                        yield return new Token(_Location, TokenType.CurlyBracketClose, character.ToString());
                        break;
                    case ';' when Advance():
                        yield return new Token(_Location, TokenType.StatementClosure, character.ToString());
                        break;
                    case '.' when Advance():
                        yield return new Token(_Location, TokenType.StatementConcat, character.ToString());
                        break;
                    case ',' when Advance():
                        yield return new Token(_Location, TokenType.ArgumentSeparator, character.ToString());
                        break;
                    case '/' when _Data[_RunIndex + 1] == '/':
                        SkipLine();
                        break;
                    case '/' when _Data[_RunIndex + 1] == '*':
                        SkipUntilCommentClosure();
                        break;
                    case '/' when Advance():
                    case '*' when Advance():
                    case '+' when Advance():
                    case '-' when Advance():
                        yield return new Token(_Location, TokenType.Operator, character.ToString());
                        break;
                    case {} when IsWhiteSpace(character):
                        SkipWhiteSpace();
                        break;
                    case {} when IsNewLine(character):
                        SkipNewLine();
                        _Location = new Point(_Location.X + 1, 1); // update location, as we've dropped to a new line
                        break;
                    case {} when IsCharacterLiteralEnclosure(character):
                        yield return new Token(_Location, TokenType.CharacterLiteral, CharacterLiteralClosure());
                        break;
                    case {} when IsStringLiteralEnclosure(character):
                        yield return new Token(_Location, TokenType.StringLiteral, StringLiteralClosure());
                        break;
                    case {} when IsDigit(character):
                        string number = NumericLiteralClosure(out bool hasDecimal);
                        yield return new Token(_Location, hasDecimal ? TokenType.DecimalLiteral : TokenType.NumericLiteral, number);
                        break;
                    case {} when IsAlphanumeric(character):
                        yield return new Token(_Location, TokenType.Identifier, AlphanumericClosure());
                        break;
                    default:
                        ExceptionHelper.Error(_Location, $"Failed to read token: {character}");
                        break;
                }
            }

            // allocate EOF
            yield return new Token(_Location, TokenType.EndOfFile, "\0");
        }

        private void SkipLine()
        {
            char character;
            while (!IsNewLine(character = Scan()) && !IsEndOfFile(character))
            {
                Advance();
            }
        }

        private void SkipUntilCommentClosure()
        {
            char character;
            while (((character = Scan()) != '*') || (_Data[_RunIndex + 1] != '/'))
            {
                if (IsEndOfFile(character))
                {
                    ExceptionHelper.Error(_Location, "Comment block has no closure.");
                }

                Advance();
            }

            // skip the '*'
            Advance();
            // and then skip the '/'
            Advance();
        }

        private void SkipWhiteSpace()
        {
            while (IsWhiteSpace(Scan()))
            {
                Advance();
            }
        }

        private void SkipNewLine()
        {
            while (IsNewLine(Scan()))
            {
                Advance();
            }
        }

        private string StringLiteralClosure()
        {
            Advance(); // advance past current string literal character

            char character;
            while (!IsStringLiteralEnclosure(character = Scan()))
            {
                if (IsEndOfFile(character))
                {
                    ExceptionHelper.Error(_Location, $"Value has no closure: {_Data[_StartIndex]}");
                }

                Advance();
            }

            string enclosedString = new string(new ArraySegment<char>(_Data, _StartIndex + 1, _RunIndex - _StartIndex - 1));

            Advance(); // move past current string literal closure

            return enclosedString;
        }

        private string CharacterLiteralClosure()
        {
            Advance(); // advance past current char literal closure character

            if (IsCharacterLiteralEnclosure(Scan()))
            {
                ExceptionHelper.Error(_Location, "Character literal has no value.");
            }

            string enclosedCharacter = _Data[_RunIndex].ToString();
            Advance(); // skip current character

            // make sure 2nd encountered char is a character literal closure
            if (!IsCharacterLiteralEnclosure(Scan()))
            {
                ExceptionHelper.Error(_Location, "Character literal can only represent a single character. Literal may have no closure.");
            }

            Advance();

            return enclosedCharacter;
        }

        private string NumericLiteralClosure(out bool hasDecimal)
        {
            bool decimalEncountered;
            hasDecimal = false;
            char character;
            while ((decimalEncountered = IsDecimal(character = Scan())) || IsDigit(character))
            {
                if (decimalEncountered)
                {
                    // second decimal point encountered, error out
                    if (hasDecimal)
                    {
                        ExceptionHelper.Error(_Location, "Decimal literal can only contain a single decimal point.");
                    }
                    else
                    {
                        hasDecimal = true;
                    }
                }

                Advance();
            }

            return GatherCurrentSubset();
        }

        private string AlphanumericClosure()
        {
            char character;
            while (IsAlphanumeric(character = Scan()) || IsDigit(character))
            {
                Advance();
            }

            return GatherCurrentSubset();
        }

        private char Scan()
        {
            if (_RunIndex < _Data.Length)
            {
                return _Data[_RunIndex];
            }
            else if (_RunIndex == _Data.Length)
            {
                return '\0';
            }
            else
            {
                ExceptionHelper.Error(_Location, "Lexer has traversed beyond the EOF token. This is a compiler error.");
                return (char)255;
            }
        }

        private bool Advance()
        {
            _RunIndex++;
            _Location.Y++;
            return true;
        }

        private string GatherCurrentSubset() => new string(new ArraySegment<char>(_Data, _StartIndex, _RunIndex - _StartIndex));

        private static bool IsEndOfFile(char character) => character == '\0';

        private static bool IsWhiteSpace(char character) => (character == ' ') || (character == '\t');

        private static bool IsNewLine(char character) => (character == '\r') || (character == '\n');

        private static bool IsDigit(char character) => (character >= '0') && (character <= '9');

        private static bool IsDecimal(char character) => character == '.';

        private static bool IsAlphanumeric(char character) =>
            ((character >= 'A') && (character <= 'Z')) || ((character >= 'a') && (character <= 'z')) || (character == '_');

        private static bool IsCharacterLiteralEnclosure(char character) => character == '\'';

        private static bool IsStringLiteralEnclosure(char character) => character == '"';
    }
}
