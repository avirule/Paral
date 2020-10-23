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
using Paral.Lexing.Tokens.Blocks;

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

                _PipeReader.AdvanceTo(consumed, consumed);
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
            else if (TryGetStringFromBuffer(buffer, "+", out bytes, out characters)) token = new OperatorToken<Add>(_Location);
            else if (TryGetStringFromBuffer(buffer, "-", out bytes, out characters)) token = new OperatorToken<Subtract>(_Location);
            else if (TryGetStringFromBuffer(buffer, "*", out bytes, out characters)) token = new OperatorToken<Multiply>(_Location);
            else if (TryGetStringFromBuffer(buffer, "/", out bytes, out characters)) token = new OperatorToken<Divide>(_Location);
            else if (TryGetStringFromBuffer(buffer, ":", out bytes, out characters)) token = new OperatorToken<RuntimeType>(_Location);
            else if (TryGetStringFromBuffer(buffer, "::", out bytes, out characters)) token = new OperatorToken<NamespaceAccessor>(_Location);

            // blocks
            else if (TryGetStringFromBuffer(buffer, "(", out bytes, out characters)) token = new ParenthesisToken(_Location, BlockTokenIntent.Open);
            else if (TryGetStringFromBuffer(buffer, ")", out bytes, out characters)) token = new ParenthesisToken(_Location, BlockTokenIntent.Close);
            else if (TryGetStringFromBuffer(buffer, "{", out bytes, out characters)) token = new BracketToken(_Location, BlockTokenIntent.Open);
            else if (TryGetStringFromBuffer(buffer, "}", out bytes, out characters)) token = new BracketToken(_Location, BlockTokenIntent.Close);

            // separators
            else if (TryGetStringFromBuffer(buffer, ",", out bytes, out characters)) token = new SeparatorToken(_Location, SeparatorType.Comma);

            // keywords
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.REQUIRES, out bytes, out characters)) token = new KeywordToken<Requires>(_Location);
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.NAMESPACE, out bytes, out characters)) token = new KeywordToken<Namespace>(_Location);
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.IMPLEMENTS, out bytes, out characters)) token = new KeywordToken<Implements>(_Location);
            else if (TryGetStringFromBuffer(buffer, KeywordHelper.STRUCT, out bytes, out characters)) token = new KeywordToken<Struct>(_Location);

            // literals
            else if (TryCaptureNumericLiteral(buffer, out bytes, out string? literal)) token = new NumericLiteralToken(_Location, literal);
            else if (TryCaptureAlphanumeric(buffer, out bytes, out string? alphanumeric)) token = new IdentifierToken(_Location, alphanumeric);

            // match against first rune
            else if (TryGetRune(buffer, out Rune rune, out bytes))
            {
                consumed = sequence.GetPosition(bytes);

                if (Rune.IsWhiteSpace(rune)) { }
                else if (rune == (Rune)'\n')
                {
                    _Location.Y += 1;
                    _Location.X = 1;
                }
                else ThrowHelper.Throw(_Location, $"Failed to read a valid token ({rune}).");

                return false;
            }
            else return false;

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

        private bool TryCaptureAlphanumeric(ReadOnlySpan<byte> buffer, out int bytes, [NotNullWhen(true)] out string? alphanumeric) =>
            TryCaptureContinuous(buffer, Rune.IsLetterOrDigit, out bytes, out alphanumeric);

        private bool TryCaptureNumericLiteral(ReadOnlySpan<byte> buffer, out int bytes, [NotNullWhen(true)] out string? literal) =>
            TryCaptureContinuous(buffer, rune => Rune.IsDigit(rune) || rune.Equals((Rune)'.'), out bytes, out literal);

        private bool TryCaptureContinuous(ReadOnlySpan<byte> buffer, Predicate<Rune> condition, out int bytesConsumed, [NotNullWhen(true)] out string? captured)
        {
            captured = null;
            bytesConsumed = 0;
            while (TryGetRune(buffer.Slice(bytesConsumed), out Rune rune, out int consumed) && condition(rune)) bytesConsumed += consumed;

            if (bytesConsumed > 0)
            {
                captured = Encoding.UTF8.GetString(buffer.Slice(0, bytesConsumed));
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
