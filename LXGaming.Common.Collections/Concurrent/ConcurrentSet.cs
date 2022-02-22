using System.Collections.Generic;

namespace LXGaming.Common.Collections.Concurrent {

    public class ConcurrentSet<T> : ConcurrentCollection<ISet<T>, T>, ISet<T> {

        public ConcurrentSet(ISet<T> set)
            : base(set) {
        }

        public new bool Add(T item) {
            Lock.EnterWriteLock();
            try {
                return Collection.Add(item);
            } finally {
                Lock.ExitWriteLock();
            }
        }

        public void ExceptWith(IEnumerable<T> other) {
            Lock.EnterWriteLock();
            try {
                Collection.ExceptWith(other);
            } finally {
                Lock.ExitWriteLock();
            }
        }

        public void IntersectWith(IEnumerable<T> other) {
            Lock.EnterWriteLock();
            try {
                Collection.IntersectWith(other);
            } finally {
                Lock.ExitWriteLock();
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other) {
            Lock.EnterReadLock();
            try {
                return Collection.IsProperSubsetOf(other);
            } finally {
                Lock.ExitReadLock();
            }
        }

        public bool IsProperSupersetOf(IEnumerable<T> other) {
            Lock.EnterReadLock();
            try {
                return Collection.IsProperSupersetOf(other);
            } finally {
                Lock.ExitReadLock();
            }
        }

        public bool IsSubsetOf(IEnumerable<T> other) {
            Lock.EnterReadLock();
            try {
                return Collection.IsSubsetOf(other);
            } finally {
                Lock.ExitReadLock();
            }
        }

        public bool IsSupersetOf(IEnumerable<T> other) {
            Lock.EnterReadLock();
            try {
                return Collection.IsSupersetOf(other);
            } finally {
                Lock.ExitReadLock();
            }
        }

        public bool Overlaps(IEnumerable<T> other) {
            Lock.EnterReadLock();
            try {
                return Collection.Overlaps(other);
            } finally {
                Lock.ExitReadLock();
            }
        }

        public bool SetEquals(IEnumerable<T> other) {
            Lock.EnterReadLock();
            try {
                return Collection.SetEquals(other);
            } finally {
                Lock.ExitReadLock();
            }
        }

        public void SymmetricExceptWith(IEnumerable<T> other) {
            Lock.EnterWriteLock();
            try {
                Collection.SymmetricExceptWith(other);
            } finally {
                Lock.ExitWriteLock();
            }
        }

        public void UnionWith(IEnumerable<T> other) {
            Lock.EnterWriteLock();
            try {
                Collection.UnionWith(other);
            } finally {
                Lock.ExitWriteLock();
            }
        }
    }
}