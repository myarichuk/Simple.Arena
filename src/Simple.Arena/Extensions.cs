using System;
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
    }
}
