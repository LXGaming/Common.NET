using System.Collections.Generic;

namespace LXGaming.Common.Collections.Concurrent {

    public class ConcurrentHashSet<T> : ConcurrentSet<T> {

        public ConcurrentHashSet()
            : base(new HashSet<T>()) {
        }

        public ConcurrentHashSet(int capacity)
            : base(new HashSet<T>(capacity)) {
        }

        public ConcurrentHashSet(IEqualityComparer<T> comparer)
            : base(new HashSet<T>(comparer)) {
        }

        public ConcurrentHashSet(int capacity, IEqualityComparer<T> comparer)
            : base(new HashSet<T>(capacity, comparer)) {
        }
    }
}