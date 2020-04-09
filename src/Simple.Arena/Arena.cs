﻿using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Simple.Arena
{
    /// <summary>
    /// Simple arena memory allocator. Works by allocating a block, then handing parts of the block as needed. When disposed, frees all memory at once
    /// </summary>
    /// <remarks>
    /// * If the Arena is not disposed, memory will leak
    /// * The Arena is NOT thread safe
    /// </remarks>
    public unsafe class Arena : IDisposable
    {
        private byte* _memoryBlock;
        private const int DefaultBlockSize = 1024 * 1024 * 1; //default size = 1MB

        /// <summary>
        /// How much memory was already allocated
        /// </summary>
        public int AllocatedBytes { get; private set; }

        /// <summary>
        /// Total memory of the Arena
        /// </summary>
        public int TotalBytes { get; private set; }

        /// <summary>
        /// If true, the Arena has been disposed and cannot be used again
        /// </summary>
        public bool IsDisposed { get; private set; }

        /// <summary>
        /// How much times the Arena was reset. This is useful for tracking possible memory leaks.
        /// </summary>
        public int ResetCount { get; private set; }

        /// <summary>
        /// Initialize the Arena allocator. 
        /// </summary>
        /// <param name="initialBlockSize">Size of the memory block the allocator will use</param>
        public Arena(int initialBlockSize = DefaultBlockSize)
        {
            if(initialBlockSize <= 0)
                Throw.ArgumentOutOfRange(nameof(initialBlockSize), "The argument must be larger than zero");

            _memoryBlock = (byte*)Marshal.AllocHGlobal(initialBlockSize).ToPointer();
            TotalBytes = initialBlockSize;
        }

        /// <summary>
        /// Allocate memory segment from Arena's pre-allocated block
        /// </summary>
        /// <param name="size">size of the segment to allocate (in bytes)</param>
        /// <param name="segment">pointer to allocated segment and its size in bytes</param>
        /// <returns>true if allocation succeeded, false otherwise</returns>
        public bool TryAllocateBytes(int size, out MemorySegment segment)
        {
            segment = default;
            if (size + AllocatedBytes > TotalBytes)
                return false;

            try
            {
                segment = AllocateInternal(size);
            }
            catch(OutOfMemoryException)
            {
                return false;
            }

            return true;
        }

        #if NETSTANDARD2_1

        /// <summary>
        /// Allocate memory segment from Arena's pre-allocated block
        /// </summary>
        /// <param name="size">size of the segment to allocate (in bytes)</param>
        /// <param name="segment">pointer to allocated segment and its size in bytes</param>
        /// <returns>true if allocation succeeded, false otherwise</returns>
        public bool TryAllocate<T>(int size, out Span<T> segment) where T : unmanaged
        {
            if (IsDisposed)
                Throw.ObjectDisposed(nameof(Arena));

            segment = default;
            int allocationSize = size * Unsafe.SizeOf<T>();
            if (allocationSize + AllocatedBytes > TotalBytes)
                return false;

            segment = MemoryMarshal.Cast<byte, T>(new Span<byte>(_memoryBlock + AllocatedBytes, allocationSize));
            AllocatedBytes += allocationSize;

            return true;
        }

        /// <summary>
        /// Allocate memory segment from Arena's pre-allocated block
        /// </summary>
        /// <typeparam name="T">type of each item to allocate in the memory segment</typeparam>
        /// <param name="size">amount of items of T to allocate</param>
        /// <returns>pointer to allocated segment and its size in bytes</returns>
        public Span<T> Allocate<T>(int size) where T : unmanaged
        {
            if (IsDisposed)
                Throw.ObjectDisposed(nameof(Arena));   
            
            int allocationSize = size * Unsafe.SizeOf<T>();
            if (allocationSize + AllocatedBytes > TotalBytes)
                Throw.OutOfMemory($"Not enough memory left in the Arena. Asked for {allocationSize} bytes, but only {TotalBytes - AllocatedBytes} bytes is available");

            var segment = MemoryMarshal.Cast<byte, T>(new Span<byte>(_memoryBlock + AllocatedBytes, allocationSize));
            AllocatedBytes += allocationSize;

            return segment;
        }

        #endif

        /// <summary>
        /// Allocate memory segment from Arena's pre-allocated block
        /// </summary>
        /// <param name="size">amount of bytes to allocate</param>
        /// <returns>pointer to allocated segment and its size in bytes</returns>
        public MemorySegment AllocateBytes(int size)
        {
            if (IsDisposed)
                Throw.ObjectDisposed(nameof(Arena));

            if (size + AllocatedBytes > TotalBytes)
                Throw.OutOfMemory($"Not enough memory left in the Arena. Asked for {size} bytes, but only {TotalBytes - AllocatedBytes} bytes is available");

            return AllocateInternal(size);
        }

        private MemorySegment AllocateInternal(int size)
        {
            var segment = new MemorySegment(_memoryBlock + AllocatedBytes, size);
            AllocatedBytes += size;

            return segment;
        }

        /// <summary>
        /// Reset the Arena, useful to reuse the Arena's in a post
        /// </summary>
        public void Reset()
        {
            if (IsDisposed)
                Throw.ObjectDisposed(nameof(Arena));
            
            ResetCount++;
            Unsafe.InitBlock(_memoryBlock, 0, (uint)TotalBytes);
            AllocatedBytes = 0;
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if(!IsDisposed)
            {
                Marshal.FreeHGlobal(new IntPtr(_memoryBlock));
                AllocatedBytes = 0;
                IsDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Arena()
        {
            Dispose(false);
        }
    }
}
