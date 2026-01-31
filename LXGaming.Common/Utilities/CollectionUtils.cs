using System.Numerics;

namespace LXGaming.Common.Utilities;

public static class CollectionUtils {

    public static void AddIgnoreNullOrEmpty(ICollection<string> collection, string? value) {
        if (!string.IsNullOrEmpty(value)) {
            collection.Add(value);
        }
    }

    public static void AddIgnoreNull<T>(ICollection<T> collection, T? value) {
        if (value != null) {
            collection.Add(value);
        }
    }

    public static void AddIgnoreNullOrEmpty<TKey>(IDictionary<TKey, string> dictionary, TKey key, string? value) {
        if (!string.IsNullOrEmpty(value)) {
            dictionary.Add(key, value);
        }
    }

    public static bool TryAddIgnoreNullOrEmpty<TKey>(IDictionary<TKey, string> dictionary, TKey key, string? value) {
        return !string.IsNullOrEmpty(value) && dictionary.TryAdd(key, value);
    }

    public static void AddIgnoreNull<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue? value) {
        if (value != null) {
            dictionary.Add(key, value);
        }
    }

    public static bool TryAddIgnoreNull<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue? value) {
        return value != null && dictionary.TryAdd(key, value);
    }

    public static void AddOrDecrement<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        where TValue : INumber<TValue> {
        if (dictionary.TryGetValue(key, out var existingValue)) {
            dictionary[key] = existingValue - value;
        } else {
            dictionary[key] = value;
        }
    }

    public static void AddOrIncrement<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue value)
        where TValue : INumber<TValue> {
        if (dictionary.TryGetValue(key, out var existingValue)) {
            dictionary[key] = existingValue + value;
        } else {
            dictionary[key] = value;
        }
    }

    public static void AddRange<T>(ICollection<T> collection, params T[] items) {
        AddRange(collection, (IEnumerable<T>) items);
    }

    public static void AddRange<T>(ICollection<T> collection, IEnumerable<T> items) {
        foreach (var item in items) {
            collection.Add(item);
        }
    }

    public static bool Set<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key, TValue? value) {
        if (value == null) {
            return dictionary.Remove(key);
        }

        dictionary[key] = value;
        return true;
    }
}