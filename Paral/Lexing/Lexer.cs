#region

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using Paral.Exceptions;
using Paral.Lexing.Tokens;

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
            else if (rune.Equals(RuneHelper.Blocks.ParenthesisOpen)) token = new BlockToken(_Location, BlockTypes.ParenthesisOpen);
            else if (rune.Equals(RuneHelper.Blocks.ParenthesisClose)) token = new BlockToken(_Location, BlockTypes.ParenthesisClose);
            else if (rune.Equals(RuneHelper.Blocks.BracketOpen)) token = new BlockToken(_Location, BlockTypes.BracketOpen);
            else if (rune.Equals(RuneHelper.Blocks.BracketClose)) token = new BlockToken(_Location, BlockTypes.BracketClose);
            else if (rune.Equals(RuneHelper.Blocks.SquareBracketOpen)) token = new BlockToken(_Location, BlockTypes.SquareBracketOpen);
            else if (rune.Equals(RuneHelper.Blocks.SquareBracketClose)) token = new BlockToken(_Location, BlockTypes.SquareBracketClose);
            // separators
            else if (rune.Equals(RuneHelper.Comma)) token = new SeparatorToken(_Location, SeparatorType.Comma);
            // numeric
            else if (Rune.IsDigit(rune) && TryCaptureContinuous(buffer, r => Rune.IsDigit(r) || r.Equals(RuneHelper.Period),
                ref bytesConsumed, out string numeric)) token = new NumericLiteralToken(_Location, numeric);
            // alphanumeric
            else if (Rune.IsLetter(rune) && TryCaptureContinuous(buffer, Rune.IsLetterOrDigit, ref bytesConsumed, out string alphanumeric))
                token = alphanumeric switch
                {
                    KeywordHelper.REQUIRES => new KeywordToken(_Location, Keyword.Requires),
                    KeywordHelper.DECLARES => new KeywordToken(_Location, Keyword.Declares),
                    KeywordHelper.IMPLEMENTS => new KeywordToken(_Location, Keyword.Implements),
                    KeywordHelper.THROWS => new KeywordToken(_Location, Keyword.Throws),
                    KeywordHelper.STRUCT => new KeywordToken(_Location, Keyword.Struct),
                    KeywordHelper.RETURNS => new KeywordToken(_Location, Keyword.Returns),
                    KeywordHelper.RETURN => new KeywordToken(_Location, Keyword.Return),
                    _ => new IdentifierToken(_Location, alphanumeric)
                };
            // other
            else if (rune.Equals(RuneHelper.Colon))
            {
                if (TryGetRune(buffer.Slice(bytesConsumed), out Rune nextRune, out int nextBytesConsumed) && nextRune.Equals(RuneHelper.Colon))
                {
                    token = new AccessOperatorToken(_Location, AccessOperator.Namespace);
                    bytesConsumed += nextBytesConsumed;
                }
                else { }
            }
            else
            {
                ThrowHelper.Throw(_Location, $"Failed to read a valid token ({rune}).");
                return false;
            }

            consumed = sequence.GetPosition(bytesConsumed);
            return true;
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

        private bool TryCaptureContinuous(ReadOnlySpan<byte> buffer, Predicate<Rune> condition, ref int bytesConsumed, out string captured)
        {
            bool success;
            while ((success = TryGetRune(buffer.Slice(bytesConsumed), out Rune rune, out int consumed)) && condition(rune)) bytesConsumed += consumed;
            captured = Encoding.UTF8.GetString(buffer.Slice(0, bytesConsumed));
            return success;
        }
    }
}
