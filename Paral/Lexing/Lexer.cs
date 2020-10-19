#region

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.IO.Pipelines;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

#endregion

namespace Paral.Lexing
{
    public class Lexer
    {
        private readonly PipeReader _PipeReader;

        private Point _Location;
        private int _Index;

        public Lexer(Stream stream)
        {
            _PipeReader = PipeReader.Create(stream);
            _Location = new Point(1, 1);
            _Index = 0;
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

            yield return new Token(_Location, TokenType.EndOfFile, "\0");
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
                NewLine();

                return false;
            }
            else if (rune.Equals(RuneHelper.Operators.Add)) token = new Token(_Location, TokenType.Operator, rune.ToString());
            else if (rune.Equals(RuneHelper.Operators.Subtract)) token = new Token(_Location, TokenType.Operator, rune.ToString());
            else if (rune.Equals(RuneHelper.Operators.Multiply)) token = new Token(_Location, TokenType.Operator, rune.ToString());
            else if (rune.Equals(RuneHelper.Operators.Divide)) token = new Token(_Location, TokenType.Operator, rune.ToString());
            else if (Rune.IsLetter(rune))
            {
                string alphanumeric = CaptureContinuous(buffer, ref bytesConsumed, Rune.IsLetterOrDigit);
                token = new Token(_Location, TokenType.Identifier, alphanumeric);
                consumed = sequence.GetPosition(bytesConsumed);
            }
            else return false;

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

        private string CaptureContinuous(ReadOnlySpan<byte> buffer, ref int bytesConsumed, Predicate<Rune> condition)
        {
            while (TryGetRune(buffer.Slice(bytesConsumed), out Rune rune, out int consumed) && condition(rune))
            {
                bytesConsumed += consumed;
            }

            return Encoding.UTF8.GetString(buffer.Slice(0, bytesConsumed));

        }

        private void NewLine()
        {
            _Location.Y += 1;
            _Location.X = 1;
        }
    }
}
