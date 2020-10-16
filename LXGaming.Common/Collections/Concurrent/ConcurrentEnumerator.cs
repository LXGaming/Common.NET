using System;
using System.Collections;
using System.Collections.Generic;

namespace LXGaming.Common.Collections.Concurrent {

    public class ConcurrentEnumerator<T> : IEnumerator<T> {

        private readonly IEnumerator<T> _enumerator;
        private readonly IDisposable _lock;

        public ConcurrentEnumerator(IEnumerable<T> enumerable, IDisposable @lock) {
            _enumerator = enumerable.GetEnumerator();
            _lock = @lock;
        }

        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset() => _enumerator.Reset();

        object IEnumerator.Current => Current;

        public void Dispose() {
            _enumerator.Dispose();
            _lock.Dispose();
        }

        public T Current => _enumerator.Current;
    }
}