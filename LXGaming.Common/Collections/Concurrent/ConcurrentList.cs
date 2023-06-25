namespace LXGaming.Common.Collections.Concurrent {

    public class ConcurrentList<T> : ConcurrentCollection<IList<T>, T>, IList<T> {

        public ConcurrentList()
            : base(new List<T>()) {
        }

        public ConcurrentList(int capacity)
            : base(new List<T>(capacity)) {
        }

        public int IndexOf(T item) {
            Lock.EnterReadLock();
            try {
                return Collection.IndexOf(item);
            } finally {
                Lock.ExitReadLock();
            }
        }

        public void Insert(int index, T item) {
            Lock.EnterWriteLock();
            try {
                Collection.Insert(index, item);
            } finally {
                Lock.ExitWriteLock();
            }
        }

        public void RemoveAt(int index) {
            Lock.EnterWriteLock();
            try {
                Collection.RemoveAt(index);
            } finally {
                Lock.ExitWriteLock();
            }
        }

        public T this[int index] {
            get {
                Lock.EnterReadLock();
                try {
                    return Collection[index];
                } finally {
                    Lock.ExitReadLock();
                }
            }
            set {
                Lock.EnterWriteLock();
                try {
                    Collection[index] = value;
                } finally {
                    Lock.ExitWriteLock();
                }
            }
        }
    }
}