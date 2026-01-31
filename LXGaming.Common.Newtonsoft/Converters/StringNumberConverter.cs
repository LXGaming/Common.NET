using System.Globalization;
using System.Numerics;
using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

public class StringNumberConverter<T>(NumberStyles? style, IFormatProvider? provider) : JsonConverter<T?>
    where T : struct, INumber<T> {

    public StringNumberConverter(NumberStyles style) : this(style, null) {
        // no-op
    }

    public StringNumberConverter() : this(null, null) {
        // no-op
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer) {
        if (value == null) {
            writer.WriteNull();
        } else {
            writer.WriteValue(value.ToString());
        }
    }

    /// <inheritdoc />
    public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue,
        JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Null) {
            return null;
        }

        if (reader.TokenType == JsonToken.String) {
            var value = (string) reader.Value!;
            if (style.HasValue) {
                return T.Parse(value, style.Value, provider);
            }

            return T.Parse(value, provider);
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing {objectType.Name}.");
    }
}