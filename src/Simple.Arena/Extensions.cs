using System;
using System.Runtime.InteropServices;

namespace Simple.Arena
{
    public static class Extensions
    {
        /// <summary>
        /// Get pointer on what Span points to. Note: this is a dangerous method - if used on a managed pointer, it may become invalid after a GC. Use only on unmanaged pointers.
        /// </summary>
        public unsafe static IntPtr ToIntPtr<T>(this Span<T> span) where T: unmanaged
        {
            fixed(T* ptr = &MemoryMarshal.GetReference(span))
                return new IntPtr(ptr);
        }

        /// <summary>
        /// Get pointer on what Span points to. Note: this is a dangerous method - if used on a managed pointer, it may become invalid after a GC. Use only on unmanaged pointers.
        /// </summary>
        public unsafe static IntPtr ToIntPtr<T>(this ReadOnlySpan<T> span) where T: unmanaged
        {
            fixed(T* ptr = &MemoryMarshal.GetReference(span))
                return new IntPtr(ptr);
        }
    }
}
