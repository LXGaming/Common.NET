namespace LXGaming.Common.Utilities;

public static class CollectionUtils {

    public static void AddIgnoreNull<T>(ICollection<T> collection, T? value) {
        if (value != null) {
            collection.Add(value);
        }
    }

    public static void AddIgnoreNull<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue? value) {
        if (value != null) {
            dictionary.Add(key, value);
        }
    }

    public static bool TryAddIgnoreNull<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue? value) {
        return value != null && dictionary.TryAdd(key, value);
    }

    public static void AddRange<T>(ICollection<T> collection, IEnumerable<T> items) {
        foreach (var item in items) {
            collection.Add(item);
        }
    }
}