using Microsoft.Extensions.ObjectPool;
using System;

namespace Simple.Arena
{
    public class ArenaPool : IDisposable
    {
        private readonly ArenaPoolPolicy _policy = new ArenaPoolPolicy(ArenaConventions.InitialArenaSize);
        private readonly DefaultObjectPool<Arena> _pool;

        public ArenaPool() => _pool = new DefaultObjectPool<Arena>(_policy);

        public IDisposable Allocate(out Arena arena)
        {
            arena = _pool.Get();
            return new DisposableAction(Return, arena);
        }

        private void Return(Arena arena) => _pool.Return(arena);


        private class DisposableAction : IDisposable
        {
            private readonly Action<Arena> _disposableAction;
            private readonly Arena _arena;

            public DisposableAction(Action<Arena> disposableAction, Arena arena)
            {
                _disposableAction = disposableAction;
                _arena = arena;
            }

            public void Dispose() => _disposableAction?.Invoke(_arena);
        }

        #region IDisposable Support
        private bool isDisposed = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                _policy.Dispose();
                isDisposed = true;
            }
        }

        ~ArenaPool()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
