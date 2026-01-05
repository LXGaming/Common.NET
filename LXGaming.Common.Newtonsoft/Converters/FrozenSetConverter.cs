using System.Collections.Frozen;
using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

public class FrozenSetConverter<T>(IEqualityComparer<T>? comparer) : JsonConverter<FrozenSet<T>?> {

    public FrozenSetConverter() : this(null) {
        // no-op
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, FrozenSet<T>? value, JsonSerializer serializer) {
        serializer.Serialize(writer, value?.ToList());
    }

    /// <inheritdoc />
    public override FrozenSet<T>? ReadJson(JsonReader reader, Type objectType, FrozenSet<T>? existingValue,
        bool hasExistingValue, JsonSerializer serializer) {
        return serializer.Deserialize<List<T>?>(reader)?.ToFrozenSet(comparer);
    }
}