#region

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using Paral.Lexing.Tokens;
using Path = Paral.Lexing.Tokens.Path;

#endregion


namespace Paral.Lexing
{
    public class Lexer
    {
        private readonly PipeReader _PipeReader;

        private Point _Location;

        public Lexer(Stream stream)
        {
            _PipeReader = PipeReader.Create(stream);
            _Location = new Point(1, 1);
        }

        public async IAsyncEnumerable<Token> Tokenize()
        {
            ReadResult result;

            while (!(result = await _PipeReader.ReadAsync()).IsCompleted)
            {
                ReadOnlySequence<byte> sequence = result.Buffer;

                if (TryReadToken(sequence, out SequencePosition consumed, out Token? token)) yield return token;

                _PipeReader.AdvanceTo(consumed);
            }

            yield return new EOFToken(_Location);
        }

        private bool TryReadToken(ReadOnlySequence<byte> sequence, out SequencePosition consumed, [NotNullWhen(true)] out Token? token)
        {
            ReadOnlySpan<byte> buffer = sequence.FirstSpan;
            consumed = sequence.Start;
            token = null;

            if (TryGetStringFromBuffer(buffer, ";", out int bytes, out int characters)) token = new TerminatorToken(_Location);

            // operators
            else if (TryGetStringFromBuffer(buffer, "+", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Add);
            else if (TryGetStringFromBuffer(buffer, "-", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Subtract);
            else if (TryGetStringFromBuffer(buffer, "*", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Multiply);
            else if (TryGetStringFromBuffer(buffer, "/", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Divide);
            else if (TryGetStringFromBuffer(buffer, "=", out bytes, out characters)) token = new OperatorToken(_Location, Operator.Assign);
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.EQUAL, out bytes, out characters)) token = new OperatorToken(_Location, Operator.Compare);

            // blocks
            else if (TryGetStringFromBuffer(buffer, "(", out bytes, out characters)) token = new GroupToken<Parenthetic, Open>(_Location);
            else if (TryGetStringFromBuffer(buffer, ")", out bytes, out characters)) token = new GroupToken<Parenthetic, Close>(_Location);
            else if (TryGetStringFromBuffer(buffer, "{", out bytes, out characters)) token = new GroupToken<Brace, Open>(_Location);
            else if (TryGetStringFromBuffer(buffer, "}", out bytes, out characters)) token = new GroupToken<Brace, Close>(_Location);

            // separators
            else if (TryGetStringFromBuffer(buffer, "::", out bytes, out characters)) token = new SeparatorToken<Path>(_Location);
            else if (TryGetStringFromBuffer(buffer, ".", out bytes, out characters)) token = new SeparatorToken<Member>(_Location);
            else if (TryGetStringFromBuffer(buffer, ",", out bytes, out characters)) token = new SeparatorToken<Comma>(_Location);

            // keywords
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.REQUIRES, out bytes, out characters)) token = new KeywordToken<Requires>(_Location);
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.NAMESPACE, out bytes, out characters)) token = new KeywordToken<Namespace>(_Location);
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.IMPLEMENTS, out bytes, out characters)) token = new KeywordToken<Implements>(_Location);
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.STRUCT, out bytes, out characters)) token = new KeywordToken<Struct>(_Location);
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.FUNCTION, out bytes, out characters)) token = new KeywordToken<Function>(_Location);
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.MUTABLE, out bytes, out characters)) token = new KeywordToken<Mutable>(_Location);
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.IMMUTABLE, out bytes, out characters)) token = new KeywordToken<Immutable>(_Location);
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.RETURN, out bytes, out characters)) token = new KeywordToken<Return>(_Location);

            // literals
            else if (TryCaptureNumericLiteral(buffer, out bytes, out characters, out string? literal))
                token = new LiteralToken(_Location, Literal.Numeric, literal);

            // identifier
            else if (TryCaptureAlphanumeric(buffer, out bytes, out characters, out string? alphanumeric)) token = new IdentifierToken(_Location, alphanumeric);

            // match against first rune
            else if (TryGetStringFromBuffer(buffer, "\r\n", out bytes, out characters) || TryGetStringFromBuffer(buffer, "\n", out bytes, out characters))
            {
                consumed = sequence.GetPosition(bytes);
                _Location.Y += 1;
                _Location.X = 1;

                return false;
            }
            else if (TryGetRune(buffer, out Rune rune, out bytes) && Rune.IsWhiteSpace(rune))
            {
                consumed = sequence.GetPosition(bytes);

                return false;
            }
            else throw new InvalidTokenException(_Location, rune);

            consumed = sequence.GetPosition(bytes);
            _Location.X += characters;
            return true;
        }

        private static bool TryGetStringFromBuffer(ReadOnlySpan<byte> buffer, string str, out int bytes, out int characters)
        {
            bytes = 0;
            characters = 0;

            foreach (Rune rune in str.EnumerateRunes())
            {
                if ((buffer.Length > bytes)
                    && (Rune.DecodeFromUtf8(buffer.Slice(bytes), out Rune result, out int readBytes) == OperationStatus.Done)
                    && (rune == result))
                {
                    bytes += readBytes;
                    characters += 1;
                }
                else return false;
            }

            return true;
        }

        private static bool TryCaptureAlphanumeric(ReadOnlySpan<byte> buffer, out int bytes, out int characters,
            [NotNullWhen(true)] out string? alphanumeric) => TryCaptureContinuous(buffer, Rune.IsLetterOrDigit, out bytes, out characters, out alphanumeric);

        private static bool TryCaptureNumericLiteral(ReadOnlySpan<byte> buffer, out int bytes, out int characters, [NotNullWhen(true)] out string? literal) =>
            TryCaptureContinuous(buffer, rune => Rune.IsDigit(rune) || rune.Equals((Rune)'.'), out bytes, out characters, out literal);

        private static bool TryCaptureContinuous(ReadOnlySpan<byte> buffer, Predicate<Rune> condition, out int bytes, out int characters,
            [NotNullWhen(true)] out string? captured)
        {
            captured = null;
            bytes = characters = 0;

            while ((Rune.DecodeFromUtf8(buffer.Slice(bytes), out Rune rune, out int bytesConsumed) == OperationStatus.Done) && condition(rune))
            {
                bytes += bytesConsumed;
                characters += 1;
            }

            if (bytes > 0)
            {
                captured = Encoding.UTF8.GetString(buffer.Slice(0, bytes));
                return true;
            }
            else return false;
        }

        private bool TryGetRune(ReadOnlySpan<byte> buffer, out Rune rune, out int bytes)
        {
            if (Rune.DecodeFromUtf8(buffer, out rune, out bytes) == OperationStatus.Done)
            {
                _Location.X += 1;

                return true;
            }
            else return false;
        }
    }
}
