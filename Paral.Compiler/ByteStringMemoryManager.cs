using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Paral.Compiler
{
    public unsafe class ByteStringMemoryManager : MemoryManager<byte>
    {
        private readonly string _String;

        public ByteStringMemoryManager(string @string) => _String = @string;

        public override Span<byte> GetSpan() => MemoryMarshal.CreateSpan(ref Unsafe.As<char, byte>(ref _String.FirstChar()), _String.Length * 2);
        public override MemoryHandle Pin(int elementIndex = 0) => new MemoryHandle((byte*)_String.AsPointer() + elementIndex);

        public override void Unpin() => throw new NotImplementedException();
        protected override void Dispose(bool disposing) => throw new NotImplementedException();
    }
}
