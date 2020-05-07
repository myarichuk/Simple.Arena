using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Simple.Arena
{
    public static class Extensions
    {
        public unsafe static IntPtr ToIntPtr<T>(this Span<T> span) where T: unmanaged
        {
            fixed(T* ptr = &MemoryMarshal.GetReference(span))
                return new IntPtr(ptr);
        }

        public unsafe static IntPtr ToIntPtr<T>(this ReadOnlySpan<T> span) where T: unmanaged
        {
            fixed(T* ptr = &MemoryMarshal.GetReference(span))
                return new IntPtr(ptr);
        }

        public unsafe static Span<T> ToSpan<T>(this IntPtr ptr, int? size = null) where T: unmanaged
        {
            byte* p = (byte*)ptr.ToPointer();
            return MemoryMarshal.Cast<byte, T>(new Span<byte>(p, size ?? Unsafe.SizeOf<T>()));
        }
    }
}
