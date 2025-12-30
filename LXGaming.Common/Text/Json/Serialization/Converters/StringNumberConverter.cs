using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Text.Json.Serialization.Converters;

public class StringNumberConverter<T> : JsonConverter<T?>
    where T : struct, IComparable, IConvertible, ISpanFormattable, IComparable<T>, IEquatable<T> {

    /// <inheritdoc />
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.Null) {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String) {
            return (T?) Convert.ChangeType(reader.GetString(), typeof(T));
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing {typeof(T).Name}.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options) {
        if (value != null) {
            writer.WriteStringValue(value.ToString());
        } else {
            writer.WriteNullValue();
        }
    }
}