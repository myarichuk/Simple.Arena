using System;
using System.Runtime.CompilerServices;

//credit: taken from https://github.com/mgravell/Pipelines.Sockets.Unofficial/blob/master/src/Pipelines.Sockets.Unofficial/Internal/Throw.cs

namespace Simple.Arena
{
    internal static class Throw
    {
        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ObjectDisposed(string objectName)
            => throw new ObjectDisposedException(objectName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void InvalidOperation(string message = null)
        {
            if (string.IsNullOrWhiteSpace(message)) 
                throw new InvalidOperationException();
            else 
                throw new InvalidOperationException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void OutOfMemory(string message = null)
        {
            if (string.IsNullOrWhiteSpace(message)) 
                throw new OutOfMemoryException();
            else 
                throw new OutOfMemoryException(message);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ArgumentOutOfRange(string paramName)
            => throw new ArgumentOutOfRangeException(paramName);

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ArgumentOutOfRange(string paramName, string message)
            => throw new ArgumentOutOfRangeException(paramName, message);

        [MethodImpl(MethodImplOptions.NoInlining)]
        internal static void ArgumentNull(string paramName)
            => throw new ArgumentNullException(paramName);
    }
}
