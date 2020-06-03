#region

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Paral.Exceptions;

#endregion

namespace Paral.Lexing
{
    public class Lexer
    {
        private readonly StreamReader _Input;

        private char[] _CurrentData;
        private Point _Location;
        private int _Index;

        public Lexer(Stream stream)
        {
            _Input = new StreamReader(stream);
            _CurrentData = new char[stream.Length];
            _Location = new Point(1, 1);
            _Index = 0;
        }

        public async IAsyncEnumerable<Token> Tokenize()
        {
            _CurrentData = (await _Input.ReadToEndAsync()).ToCharArray();

            char character;
            while (!IsEndOfFile(character = Scan()))
            {
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
                    case '/' when ScanAhead() == '/':
                        SkipLine();
                        break;
                    case '/' when ScanAhead() == '*':
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
                        break;
                    case {} when IsCharacterLiteralEnclosure(character):
                        yield return new Token(_Location, TokenType.CharacterLiteral, CharacterLiteralClosure());
                        break;
                    case {} when IsStringLiteralEnclosure(character):
                        yield return new Token(_Location, TokenType.StringLiteral, StringLiteralClosure());
                        break;
                    case {} when IsDigit(character):
                        (bool hasDecimal, string literal) = NumericLiteralClosure();
                        yield return new Token(_Location, hasDecimal ? TokenType.DecimalLiteral : TokenType.NumericLiteral, literal);
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
            while (!IsNewLine(Scan()) && !IsEndOfFile(Scan()))
            {
                Advance();
            }
        }

        private void SkipUntilCommentClosure()
        {
            char character;
            while (((character = Scan()) != '*') || (ScanAhead() != '/'))
            {
                if (IsEndOfFile(character))
                {
                    ExceptionHelper.Error(_Location, "Comment block has no closure.");
                }
                else if (IsNewLine(character))
                {
                    SkipNewLine();
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

            _Location = new Point(_Location.X + 1, 1); // update location, as we've dropped to a new line
        }

        private string StringLiteralClosure()
        {
            Advance(); // advance past current string literal character

            char character;
            StringBuilder literal = new StringBuilder();
            while (!IsStringLiteralEnclosure(character = Scan()))
            {
                if (IsEndOfFile(character))
                {
                    ExceptionHelper.Error(_Location, "String literal has no closure.");
                }

                literal.Append(character);
                Advance();
            }

            Advance(); // move past current string literal closure

            return literal.ToString();
        }

        private string CharacterLiteralClosure()
        {
            Advance(); // advance past current char literal closure character

            if (IsCharacterLiteralEnclosure(Scan()))
            {
                ExceptionHelper.Error(_Location, "Character literal has no value.");
            }

            string literal = Scan().ToString();
            Advance(); // skip current character

            // make sure 2nd encountered char is a character literal closure
            if (!IsCharacterLiteralEnclosure(Scan()))
            {
                ExceptionHelper.Error(_Location, "Character literal can only represent a single character. Literal may have no closure.");
            }

            Advance();

            return literal;
        }

        private (bool, string) NumericLiteralClosure()
        {
            char character;
            bool decimalEncountered;
            bool hasDecimal = false;
            StringBuilder literal = new StringBuilder();
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

                literal.Append(character);
                Advance();
            }

            return (hasDecimal, literal.ToString());
        }

        private string AlphanumericClosure()
        {
            char character;
            StringBuilder literal = new StringBuilder();
            while (IsAlphanumeric(character = Scan()) || IsDigit(character))
            {
                literal.Append(character);
                Advance();
            }

            return literal.ToString();
        }

        private char Scan() => _Index < _CurrentData.Length ? _CurrentData[_Index] : '\0';
        private char ScanAhead() => (_Index + 1) < _CurrentData.Length ? _CurrentData[_Index + 1] : '\0';

        private bool Advance()
        {
            _Index++;
            _Location.X++;
            return true;
        }

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
