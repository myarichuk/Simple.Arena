namespace Simple.Arena
{
    using System;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// Defines the <see cref="Throw" />.
    /// </summary>
    internal static class Throw
    {
        /// <summary>
        /// The ObjectDisposed.
        /// </summary>
        /// <param name="objectName">The objectName<see cref="string"/>.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ObjectDisposed(string objectName)
            => throw new ObjectDisposedException(objectName);

        /// <summary>
        /// The InvalidOperation.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void InvalidOperation(string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new InvalidOperationException();
            else
                throw new InvalidOperationException(message);
        }

        /// <summary>
        /// The OutOfMemory.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void OutOfMemory(string message = null)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new OutOfMemoryException();
            else
                throw new OutOfMemoryException(message);
        }

        /// <summary>
        /// The ArgumentOutOfRange.
        /// </summary>
        /// <param name="paramName">The paramName<see cref="string"/>.</param>
        /// <param name="message">The message<see cref="string"/>.</param>
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ArgumentOutOfRange(string paramName, string message)
            => throw new ArgumentOutOfRangeException(paramName, message);
    }
}
