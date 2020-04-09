#if NETSTANDARD2_1
using System;
using System.Runtime.InteropServices;
#endif

namespace Simple.Arena
{
    public unsafe readonly struct MemorySegment
    {
        public readonly byte* Ptr;
        public readonly int Length;

        public MemorySegment(byte* ptr, int length)
        {
            Ptr = ptr;
            Length = length;
        }

#if NETSTANDARD2_1
        public Span<T> AsSpan<T>() where T : unmanaged =>
            MemoryMarshal.Cast<byte, T>(new Span<byte>(Ptr, Length));

        public Span<byte> AsSpan() => new Span<byte>(Ptr, Length);            
#endif
    }
}
