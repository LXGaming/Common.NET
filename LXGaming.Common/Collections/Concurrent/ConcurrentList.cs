using System.Collections.Generic;

namespace LXGaming.Common.Collections.Concurrent {

    public class ConcurrentList<T> : ConcurrentCollection<IList<T>, T>, IList<T> {

        public ConcurrentList()
            : base(new List<T>()) {
        }

        public ConcurrentList(int capacity)
            : base(new List<T>(capacity)) {
        }

        public int IndexOf(T item) {
            using (Lock.ReaderLock()) {
                return Collection.IndexOf(item);
            }
        }

        public void Insert(int index, T item) {
            using (Lock.WriterLock()) {
                Collection.Insert(index, item);
            }
        }

        public void RemoveAt(int index) {
            using (Lock.WriterLock()) {
                Collection.RemoveAt(index);
            }
        }

        public T this[int index] {
            get {
                using (Lock.ReaderLock()) {
                    return Collection[index];
                }
            }
            set {
                using (Lock.WriterLock()) {
                    Collection[index] = value;
                }
            }
        }
    }
}