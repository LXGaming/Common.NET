using System.Collections;
using System.Collections.Generic;
using Nito.AsyncEx;

namespace LXGaming.Common.Collections.Concurrent {

    public class ConcurrentCollection<TCollection, TItem> : ICollection<TItem> where TCollection : ICollection<TItem> {

        protected readonly TCollection Collection;
        protected readonly AsyncReaderWriterLock Lock;

        public ConcurrentCollection(TCollection collection) {
            Collection = collection;
            Lock = new AsyncReaderWriterLock();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public IEnumerator<TItem> GetEnumerator() => new ConcurrentEnumerator<TItem>(Collection, Lock.ReaderLock());

        public void Add(TItem item) {
            using (Lock.WriterLock()) {
                Collection.Add(item);
            }
        }

        public void Clear() {
            using (Lock.WriterLock()) {
                Collection.Clear();
            }
        }

        public bool Contains(TItem item) {
            using (Lock.ReaderLock()) {
                return Collection.Contains(item);
            }
        }

        public void CopyTo(TItem[] array, int arrayIndex) {
            using (Lock.ReaderLock()) {
                Collection.CopyTo(array, arrayIndex);
            }
        }

        public bool Remove(TItem item) {
            using (Lock.WriterLock()) {
                return Collection.Remove(item);
            }
        }

        public int Count {
            get {
                using (Lock.ReaderLock()) {
                    return Collection.Count;
                }
            }
        }

        public bool IsReadOnly => false;
    }
}