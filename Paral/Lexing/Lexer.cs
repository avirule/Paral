#region

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Paral.Exceptions;

#endregion

namespace Paral.Lexing
{
    public class Lexer
    {
        private readonly Stream _Input;
        private readonly byte[] _CurrentData;

        private bool _EndOfFile;
        private Point _Location;

        public Lexer(Stream stream)
        {
            _Input = stream;
            _CurrentData = new byte[2];
            _EndOfFile = false;
            _Location = new Point(1, 1);
        }

        public async IAsyncEnumerable<Token> Tokenize()
        {
            await Advance();

            char character;
            while (!IsEndOfFile(character = Scan()))
            {
                switch (character)
                {
                    case '(' when await Advance():
                        yield return new Token(_Location, TokenType.ParenthesisOpen, character.ToString());
                        break;
                    case ')' when await Advance():
                        yield return new Token(_Location, TokenType.ParenthesisClose, character.ToString());
                        break;
                    case '{' when await Advance():
                        yield return new Token(_Location, TokenType.CurlyBracketOpen, character.ToString());
                        break;
                    case '}' when await Advance():
                        yield return new Token(_Location, TokenType.CurlyBracketClose, character.ToString());
                        break;
                    case ';' when await Advance():
                        yield return new Token(_Location, TokenType.StatementClosure, character.ToString());
                        break;
                    case '.' when await Advance():
                        yield return new Token(_Location, TokenType.StatementConcat, character.ToString());
                        break;
                    case ',' when await Advance():
                        yield return new Token(_Location, TokenType.ArgumentSeparator, character.ToString());
                        break;
                    case '/' when ScanAhead() == '/':
                        await SkipLine();
                        break;
                    case '/' when ScanAhead() == '*':
                        await SkipUntilCommentClosure();
                        break;
                    case '/' when await Advance():
                    case '*' when await Advance():
                    case '+' when await Advance():
                    case '-' when await Advance():
                        yield return new Token(_Location, TokenType.Operator, character.ToString());
                        break;
                    case {} when IsWhiteSpace(character):
                        await SkipWhiteSpace();
                        break;
                    case {} when IsNewLine(character):
                        await SkipNewLine();
                        _Location = new Point(_Location.X + 1, 1); // update location, as we've dropped to a new line
                        break;
                    case {} when IsCharacterLiteralEnclosure(character):
                        yield return new Token(_Location, TokenType.CharacterLiteral, await CharacterLiteralClosure());
                        break;
                    case {} when IsStringLiteralEnclosure(character):
                        yield return new Token(_Location, TokenType.StringLiteral, await StringLiteralClosure());
                        break;
                    case {} when IsDigit(character):
                        (bool hasDecimal, string literal) = await NumericLiteralClosure();
                        yield return new Token(_Location, hasDecimal ? TokenType.DecimalLiteral : TokenType.NumericLiteral, literal);
                        break;
                    case {} when IsAlphanumeric(character):
                        yield return new Token(_Location, TokenType.Identifier, await AlphanumericClosure());
                        break;
                    default:
                        ExceptionHelper.Error(_Location, $"Failed to read token: {character}");
                        break;
                }
            }

            // allocate EOF
            yield return new Token(_Location, TokenType.EndOfFile, "\0");
        }

        private async ValueTask SkipLine()
        {
            while (!IsNewLine(Scan()) && !IsEndOfFile(Scan()))
            {
                await Advance();
            }
        }

        private async ValueTask SkipUntilCommentClosure()
        {
            char character;
            while (((character = Scan()) != '*') || (ScanAhead() != '/'))
            {
                if (IsEndOfFile(character))
                {
                    ExceptionHelper.Error(_Location, "Comment block has no closure.");
                }

                await Advance();
            }

            // skip the '*'
            await Advance();
            // and then skip the '/'
            await Advance();
        }

        private async ValueTask SkipWhiteSpace()
        {
            while (IsWhiteSpace(Scan()))
            {
                await Advance();
            }
        }

        private async ValueTask SkipNewLine()
        {
            while (IsNewLine(Scan()))
            {
                await Advance();
            }
        }

        private async ValueTask<string> StringLiteralClosure()
        {
            await Advance(); // advance past current string literal character

            char character;
            StringBuilder literal = new StringBuilder();
            while (!IsStringLiteralEnclosure(character = Scan()))
            {
                if (IsEndOfFile(character))
                {
                    ExceptionHelper.Error(_Location, "String literal has no closure.");
                }

                literal.Append(character);
                await Advance();
            }

            await Advance(); // move past current string literal closure

            return literal.ToString();
        }

        private async ValueTask<string> CharacterLiteralClosure()
        {
            await Advance(); // advance past current char literal closure character

            if (IsCharacterLiteralEnclosure(Scan()))
            {
                ExceptionHelper.Error(_Location, "Character literal has no value.");
            }

            string literal = Scan().ToString();
            await Advance(); // skip current character

            // make sure 2nd encountered char is a character literal closure
            if (!IsCharacterLiteralEnclosure(Scan()))
            {
                ExceptionHelper.Error(_Location, "Character literal can only represent a single character. Literal may have no closure.");
            }

            await Advance();

            return literal;
        }

        private async ValueTask<(bool, string)> NumericLiteralClosure()
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
                await Advance();
            }

            return (hasDecimal, literal.ToString());
        }

        private async ValueTask<string> AlphanumericClosure()
        {
            char character;
            StringBuilder literal = new StringBuilder();
            while (IsAlphanumeric(character = Scan()) || IsDigit(character))
            {
                literal.Append(character);
                await Advance();
            }

            return literal.ToString();
        }

        private char Scan() => _EndOfFile ? '\0' : (char)_CurrentData[0];
        private char ScanAhead() => _EndOfFile ? '\0' : (char)_CurrentData[1];

        private async ValueTask<bool> Advance()
        {
            int bytesRead = await _Input.ReadAsync(_CurrentData, 0, 2);
            // position will be incremented 2 when read, so step back once.
            // remark: this is for look-ahead capability.
            if (bytesRead > 1)
            {
                _Input.Position--;
            }
            else if (bytesRead <= 0)
            {
                _EndOfFile = true;
            }

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
