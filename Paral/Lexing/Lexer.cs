#region

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using Paral.Lexing.Tokens;
using Paral.Lexing.Tokens.Blocks;
using Paral.Lexing.Tokens.Keywords;
using Paral.Parsing.Nodes;
using Serilog.Core;

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

                if (TryReadToken(sequence, out SequencePosition consumed, out Token token)) yield return token;

                _PipeReader.AdvanceTo(consumed, consumed);
            }

            yield return new EOFToken(_Location);
        }

        private bool TryReadToken(ReadOnlySequence<byte> sequence, out SequencePosition consumed, out Token token)
        {
            ReadOnlySpan<byte> buffer = sequence.FirstSpan;
            consumed = sequence.Start;
            token = default!;

            if (!TryGetRune(buffer, out Rune rune, out int bytesConsumed)) return false;

            consumed = sequence.GetPosition(bytesConsumed);

            if (rune.Equals(RuneHelper.NewLine))
            {
                _Location.Y += 1;
                _Location.X = 1;

                return false;
            }
            else if (Rune.IsWhiteSpace(rune)) return false;
            else if (rune.Equals(RuneHelper.Semicolon)) token = new TerminatorToken(_Location);

            // operators
            else if (rune.Equals(RuneHelper.Operators.Add)) token = new ArithmeticOperatorToken(_Location, ArithmeticOperator.Add);
            else if (rune.Equals(RuneHelper.Operators.Subtract)) token = new ArithmeticOperatorToken(_Location, ArithmeticOperator.Subtract);
            else if (rune.Equals(RuneHelper.Operators.Multiply)) token = new ArithmeticOperatorToken(_Location, ArithmeticOperator.Multiply);
            else if (rune.Equals(RuneHelper.Operators.Divide)) token = new ArithmeticOperatorToken(_Location, ArithmeticOperator.Divide);

            // blocks
            else if (rune.Equals(RuneHelper.Blocks.ParenthesisOpen)) token = new ParenthesisToken(_Location, BlockTokenIntent.Open);
            else if (rune.Equals(RuneHelper.Blocks.ParenthesisClose)) token = new ParenthesisToken(_Location, BlockTokenIntent.Close);
            else if (rune.Equals(RuneHelper.Blocks.BracketOpen)) token = new BracketToken(_Location, BlockTokenIntent.Open);
            else if (rune.Equals(RuneHelper.Blocks.BracketClose)) token = new BracketToken(_Location, BlockTokenIntent.Close);

            // separators
            else if (rune.Equals(RuneHelper.Comma)) token = new SeparatorToken(_Location, SeparatorType.Comma);

            // numeric
            else if (Rune.IsDigit(rune) && TryCaptureNumericLiteral(buffer, ref bytesConsumed, out string literal))
                token = new NumericLiteralToken(_Location, literal);

            // alphanumeric
            else if (Rune.IsLetter(rune) && TryCaptureAlphanumeric(buffer, ref bytesConsumed, out string alphanumeric))
            {
                token = alphanumeric switch
                {
                    KeywordHelper.REQUIRES => new RequiresToken(_Location),
                    KeywordHelper.NAMESPACE => new NamespaceToken(_Location),
                    KeywordHelper.IMPLEMENTS => new ImplementsToken(_Location),
                    KeywordHelper.STRUCT => new StructToken(_Location),
                    _ => new IdentifierToken(_Location, alphanumeric)
                };
            }

            // other
            else if (rune.Equals(RuneHelper.Colon))
            {
                if (IsNextRuneEqual(buffer.Slice(bytesConsumed), RuneHelper.Colon, out int runeLength))
                {
                    token = new NamespaceAccessorToken(_Location);
                    bytesConsumed += runeLength;
                }
                else
                {
                    return false;
                    token = new TypeAssignmentOperatorToken(_Location);
                }
            }
            else
            {
                ThrowHelper.Throw(_Location, $"Failed to read a valid token ({rune}).");
                return false;
            }

            consumed = sequence.GetPosition(bytesConsumed);
            return true;
        }

        private bool IsNextRuneEqual(ReadOnlySpan<byte> buffer, Rune comparison, out int runeLength) =>
            TryGetRune(buffer, out Rune rune, out runeLength) && rune.Equals(comparison);

        private bool TryCaptureAlphanumeric(ReadOnlySpan<byte> buffer, ref int bytesConsumed, out string alphanumeric) =>
            TryCaptureContinuous(buffer, Rune.IsLetterOrDigit, ref bytesConsumed, out alphanumeric);

        private bool TryCaptureNumericLiteral(ReadOnlySpan<byte> buffer, ref int bytesConsumed, out string literal) =>
            TryCaptureContinuous(buffer, r => Rune.IsDigit(r) || r.Equals(RuneHelper.Period), ref bytesConsumed, out literal);

        private bool TryCaptureContinuous(ReadOnlySpan<byte> buffer, Predicate<Rune> condition, ref int bytesConsumed, out string captured)
        {
            bool success;
            while ((success = TryGetRune(buffer.Slice(bytesConsumed), out Rune rune, out int consumed)) && condition(rune)) bytesConsumed += consumed;

            captured = Encoding.UTF8.GetString(buffer.Slice(0, bytesConsumed));
            return success;
        }

        private bool TryGetRune(ReadOnlySpan<byte> buffer, out Rune rune, out int bytesConsumed)
        {
            if (Rune.DecodeFromUtf8(buffer, out rune, out bytesConsumed) == OperationStatus.Done)
            {
                _Location.X += 1;

                return true;
            }
            else return false;
        }
    }
}
