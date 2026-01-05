using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Text.Json.Serialization.Converters;

public class FrozenDictionaryConverter<TKey, TValue>(
    IEqualityComparer<TKey>? comparer) : JsonConverter<FrozenDictionary<TKey, TValue>?>
    where TKey : notnull {

    public FrozenDictionaryConverter() : this(null) {
        // no-op
    }

    /// <inheritdoc />
    public override FrozenDictionary<TKey, TValue>? Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options) {
        return JsonSerializer.Deserialize<Dictionary<TKey, TValue>?>(ref reader, options)?.ToFrozenDictionary(comparer);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, FrozenDictionary<TKey, TValue>? value,
        JsonSerializerOptions options) {
        JsonSerializer.Serialize<IDictionary<TKey, TValue>?>(writer, value, options);
    }
}