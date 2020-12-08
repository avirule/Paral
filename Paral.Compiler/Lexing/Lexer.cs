using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Text;
using Paral.Compiler.Lexing.Tokens;
using Path = Paral.Compiler.Lexing.Tokens.Path;

namespace Paral.Compiler.Lexing
{
    public class Lexer
    {
        private readonly StreamReader _Reader;

        private Point _Location;

        public Lexer(Stream stream)
        {
            _Reader = new StreamReader(stream);
            _Location = new Point(1, 1);
        }

        public async IAsyncEnumerable<Token> Tokenize()
        {
            while (!_Reader.EndOfStream)
            {
                string? line = await _Reader.ReadLineAsync();

                if (line is null) throw new InvalidOperationException(nameof(line));

                foreach (Token token in TokenizeLine(line)) yield return token;
            }
        }

        private IEnumerable<Token> TokenizeLine(string line)
        {
            for (int index = 0, bytesConsumed; index < line.Length; index += bytesConsumed)
                if (TryReadToken(line.Substring(index), out bytesConsumed, out Token? token))
                    yield return token;
        }

        private bool TryReadToken(string line, out int bytesConsumed, [NotNullWhen(true)] out Token? token)
        {
            bytesConsumed = 0;
            token = null;

            // terminator
            if (TryGetStringFromBuffer(line, ";", out int bytes, out int characters)) token = new TerminatorToken(_Location);

            // operators
            else if (TryGetStringFromBuffer(line, "+", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Add);
            else if (TryGetStringFromBuffer(line, "-", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Subtract);
            else if (TryGetStringFromBuffer(line, "*", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Multiply);
            else if (TryGetStringFromBuffer(line, "/", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Divide);
            else if (TryGetStringFromBuffer(line, "=", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Assign);
            else if (TryGetStringFromBuffer(line, "equal", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Compare);
            else if (TryGetStringFromBuffer(line, "is", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Compare);

            // blocks
            else if (TryGetStringFromBuffer(line, "(", out bytes, out characters)) token = new GroupToken<Paren, Open>(_Location);
            else if (TryGetStringFromBuffer(line, ")", out bytes, out characters)) token = new GroupToken<Paren, Close>(_Location);
            else if (TryGetStringFromBuffer(line, "{", out bytes, out characters)) token = new GroupToken<Brace, Open>(_Location);
            else if (TryGetStringFromBuffer(line, "}", out bytes, out characters)) token = new GroupToken<Brace, Close>(_Location);

            // separators
            else if (TryGetStringFromBuffer(line, "::", out bytes, out characters)) token = new SeparatorToken<Path>(_Location);
            else if (TryGetStringFromBuffer(line, ".", out bytes, out characters)) token = new SeparatorToken<Member>(_Location);
            else if (TryGetStringFromBuffer(line, ",", out bytes, out characters)) token = new SeparatorToken<Comma>(_Location);

            // keywords
            else if (TryGetStringFromBuffer(line, "requires", out bytes, out characters)) token = new KeywordToken<Requires>(_Location);
            else if (TryGetStringFromBuffer(line, "namespace", out bytes, out characters)) token = new KeywordToken<Namespace>(_Location);
            else if (TryGetStringFromBuffer(line, "implements", out bytes, out characters)) token = new KeywordToken<Implements>(_Location);
            else if (TryGetStringFromBuffer(line, "struct", out bytes, out characters)) token = new KeywordToken<Struct>(_Location);
            else if (TryGetStringFromBuffer(line, "function", out bytes, out characters)) token = new KeywordToken<Function>(_Location);
            else if (TryGetStringFromBuffer(line, "return", out bytes, out characters)) token = new KeywordToken<Return>(_Location);
            else if (TryGetStringFromBuffer(line, "immutable", out bytes, out characters)
                     || TryGetStringFromBuffer(line, "imm", out bytes, out characters)) token = new MutabilityToken(_Location, Mutability.Immutable);
            else if (TryGetStringFromBuffer(line, "mutable", out bytes, out characters)
                     || TryGetStringFromBuffer(line, "mut", out bytes, out characters)) token = new MutabilityToken(_Location, Mutability.Mutable);

            // literals
            else if (TryCaptureNumericLiteral(line, out bytes, out characters, out string? literal))
                token = new LiteralToken(_Location, Literal.Numeric, literal);

            // identifier
            else if (TryCaptureAlphanumeric(line, out bytes, out characters, out string? alphanumeric)) token = new IdentifierToken(_Location, alphanumeric);

            // match against first rune
            else if (TryGetStringFromBuffer(line, "\r\n", out bytes, out characters) || TryGetStringFromBuffer(line, "\n", out bytes, out characters))
            {
                bytesConsumed = bytes;
                _Location.Y += 1;
                _Location.X = 1;

                return false;
            }
            else if (TryGetRune(line, out Rune rune, out bytes) && Rune.IsWhiteSpace(rune))
            {
                bytesConsumed = bytes;

                return false;
            }
            else throw new InvalidTokenException(_Location, rune);

            bytesConsumed = bytes;
            _Location.X += characters;
            return true;
        }

        private static bool TryGetStringFromBuffer(string line, string query, out int bytes, out int characters)
        {
            bytes = 0;
            characters = 0;

            StringRuneEnumerator lineEnumerator = line.EnumerateRunes();
            StringRuneEnumerator queryEnumerator = query.EnumerateRunes();

            while (queryEnumerator.MoveNext() && lineEnumerator.MoveNext())
            {
                Rune queryEnumeratorCurrent = queryEnumerator.Current;
                Rune lineEnumeratorCurrent = lineEnumerator.Current;

                if (!queryEnumeratorCurrent.Equals(lineEnumeratorCurrent)) return false;
                else
                {
                    bytes += queryEnumeratorCurrent.Utf8SequenceLength;
                    characters += 1;
                }
            }

            return true;
        }

        private static bool TryCaptureAlphanumeric(string line, out int bytes, out int characters,
            [NotNullWhen(true)] out string? alphanumeric) => TryCaptureContinuous(line, Rune.IsLetterOrDigit, out bytes, out characters, out alphanumeric);

        private static bool TryCaptureNumericLiteral(string line, out int bytes, out int characters, [NotNullWhen(true)] out string? literal) =>
            TryCaptureContinuous(line, rune => Rune.IsDigit(rune) || rune.Equals((Rune)'.'), out bytes, out characters, out literal);

        private static bool TryCaptureContinuous(string line, Predicate<Rune> condition, out int bytes, out int characters,
            [NotNullWhen(true)] out string? captured)
        {
            captured = null;
            bytes = characters = 0;

            StringRuneEnumerator lineEnumerator = line.EnumerateRunes();

            while (lineEnumerator.MoveNext())
            {
                Rune lineEnumeratorCurrent = lineEnumerator.Current;

                if (condition(lineEnumeratorCurrent))
                {
                    bytes += lineEnumeratorCurrent.Utf8SequenceLength;
                    characters += 1;
                }
                else break;
            }

            if (bytes > 0)
            {
                captured = line.Substring(0, characters);
                return true;
            }
            else return false;
        }

        private static bool TryGetRune(string line, out Rune rune, out int bytes)
        {
            foreach (Rune enumerateRune in line.EnumerateRunes())
            {
                rune = enumerateRune;
                bytes = enumerateRune.Utf8SequenceLength;
                return true;
            }

            rune = default;
            bytes = default;
            return false;
        }
    }
}
