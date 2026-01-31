using System.Globalization;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Text.Json.Serialization.Converters;

public class StringNumberConverter<T>(NumberStyles? style, IFormatProvider? provider) : JsonConverter<T>
    where T : struct, INumber<T> {

    public StringNumberConverter(NumberStyles style) : this(style, null) {
        // no-op
    }

    public StringNumberConverter() : this(null, null) {
        // no-op
    }

    /// <inheritdoc />
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.String) {
            var value = reader.GetString()!;
            if (style.HasValue) {
                return T.Parse(value, style.Value, provider);
            }

            return T.Parse(value, provider);
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing {typeToConvert.Name}.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.ToString());
    }
}