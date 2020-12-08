using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Paral.Compiler
{
    public static class StringExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void* AsPointer(this string @string) => Unsafe.AsPointer(ref MemoryMarshal.GetReference(@string.AsSpan()));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref char FirstChar(this string @string) => ref MemoryMarshal.GetReference(@string.AsSpan());
    }
}
