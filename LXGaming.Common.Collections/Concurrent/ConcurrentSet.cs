using System.Collections.Generic;

namespace LXGaming.Common.Collections.Concurrent {

    public class ConcurrentSet<T> : ConcurrentCollection<ISet<T>, T>, ISet<T> {

        public ConcurrentSet(ISet<T> set)
            : base(set) {
        }

        public new bool Add(T item) {
            using (Lock.WriterLock()) {
                return Collection.Add(item);
            }
        }

        public void ExceptWith(IEnumerable<T> other) {
            using (Lock.WriterLock()) {
                Collection.ExceptWith(other);
            }
        }

        public void IntersectWith(IEnumerable<T> other) {
            using (Lock.WriterLock()) {
                Collection.IntersectWith(other);
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other) {
            using (Lock.ReaderLock()) {
                return Collection.IsProperSubsetOf(other);
            }
        }

        public bool IsProperSupersetOf(IEnumerable<T> other) {
            using (Lock.ReaderLock()) {
                return Collection.IsProperSupersetOf(other);
            }
        }

        public bool IsSubsetOf(IEnumerable<T> other) {
            using (Lock.ReaderLock()) {
                return Collection.IsSubsetOf(other);
            }
        }

        public bool IsSupersetOf(IEnumerable<T> other) {
            using (Lock.ReaderLock()) {
                return Collection.IsSupersetOf(other);
            }
        }

        public bool Overlaps(IEnumerable<T> other) {
            using (Lock.ReaderLock()) {
                return Collection.Overlaps(other);
            }
        }

        public bool SetEquals(IEnumerable<T> other) {
            using (Lock.ReaderLock()) {
                return Collection.SetEquals(other);
            }
        }

        public void SymmetricExceptWith(IEnumerable<T> other) {
            using (Lock.WriterLock()) {
                Collection.SymmetricExceptWith(other);
            }
        }

        public void UnionWith(IEnumerable<T> other) {
            using (Lock.WriterLock()) {
                Collection.UnionWith(other);
            }
        }
    }
}