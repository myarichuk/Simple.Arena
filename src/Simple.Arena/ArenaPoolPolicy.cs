using Microsoft.Extensions.ObjectPool;
using System;
using System.Collections.Concurrent;

namespace Simple.Arena
{
    internal class ArenaPoolPolicy : IPooledObjectPolicy<Arena>, IDisposable
    {
        private readonly ConcurrentQueue<Arena> _actualPool = new ConcurrentQueue<Arena>();
        private readonly int _defaultSize;

        public ArenaPoolPolicy(int defaultSize) => _defaultSize = defaultSize;

        public Arena Create()
        {
            if(_actualPool.TryDequeue(out var pool))
                return pool;

            return new Arena(_defaultSize);
        }

        public void Dispose()
        {
            foreach(var arena in _actualPool)
                arena.Dispose();
        }

        public bool Return(Arena obj)
        {
            obj.Reset();
            _actualPool.Enqueue(obj);
            return true;
        }
    }
}