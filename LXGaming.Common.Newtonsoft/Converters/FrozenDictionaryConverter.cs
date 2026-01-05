using System.Collections.Frozen;
using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

public class FrozenDictionaryConverter<TKey, TValue>(
    IEqualityComparer<TKey>? comparer) : JsonConverter<FrozenDictionary<TKey, TValue>?>
    where TKey : notnull {

    public FrozenDictionaryConverter() : this(null) {
        // no-op
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, FrozenDictionary<TKey, TValue>? value,
        JsonSerializer serializer) {
        serializer.Serialize(writer, value?.ToDictionary());
    }

    /// <inheritdoc />
    public override FrozenDictionary<TKey, TValue>? ReadJson(JsonReader reader, Type objectType,
        FrozenDictionary<TKey, TValue>? existingValue, bool hasExistingValue, JsonSerializer serializer) {
        return serializer.Deserialize<Dictionary<TKey, TValue>?>(reader)?.ToFrozenDictionary(comparer);
    }
}