namespace Simple.Arena
{
    using System;

    /// <summary>
    /// The common interface all arena allocators implement <see cref="IArena" />.
    /// </summary>
    public interface IArena : IDisposable
    {
        /// <summary>
        /// How much memory was already allocated
        /// </summary>
        int AllocatedBytes { get; }

        /// <summary>
        /// If true, the Arena has been disposed and cannot be used again
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// How much times the Arena was reset. This is useful for tracking possible memory leaks.
        /// </summary>
        int ResetCount { get; }

        /// <summary>
        /// Total memory of the Arena
        /// </summary>
        int TotalBytes { get; }

        /// <summary>
        /// Allocate memory segment from Arena's pre-allocated block
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="amountOfTs">The amountOfTs<see cref="int"/>.</param>
        /// <returns>The <see cref="Span{T}"/>.</returns>
        Span<T> Allocate<T>(int amountOfTs) where T : unmanaged;

        /// <summary>
        /// Allocate memory segment from Arena's pre-allocated block
        /// </summary>
        /// <typeparam name="T">.</typeparam>
        /// <param name="amountOfTs">The amountOfTs<see cref="int"/>.</param>
        /// <param name="segment">The segment<see cref="Span{T}"/>.</param>
        /// <returns>The <see cref="bool"/>.</returns>
        bool TryAllocate<T>(int amountOfTs, out Span<T> segment) where T : unmanaged;
      
        /// <summary>
        /// Reset the Arena, useful to reuse the Arena's in an object pool
        /// </summary>
        void Reset();      
    }
}
