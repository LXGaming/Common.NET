using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Text.Json.Serialization.Converters;

public class FrozenSetConverter<T>(IEqualityComparer<T>? comparer) : JsonConverter<FrozenSet<T>?> {

    public FrozenSetConverter() : this(null) {
        // no-op
    }

    /// <inheritdoc />
    public override FrozenSet<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        return JsonSerializer.Deserialize<List<T>?>(ref reader, options)?.ToFrozenSet(comparer);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, FrozenSet<T>? value, JsonSerializerOptions options) {
        JsonSerializer.Serialize<ISet<T>?>(writer, value, options);
    }
}