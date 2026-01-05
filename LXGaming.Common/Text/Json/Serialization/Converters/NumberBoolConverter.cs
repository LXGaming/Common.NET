using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Text.Json.Serialization.Converters;

public class NumberBoolConverter(int falseNumber, int trueNumber) : JsonConverter<bool> {

    public NumberBoolConverter() : this(0, 1) {
        // no-op
    }

    /// <inheritdoc />
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.Number) {
            var value = reader.GetInt32();
            if (value == falseNumber) {
                return false;
            }

            if (value == trueNumber) {
                return true;
            }

            throw new JsonException($"Integer value '{value}' is not allowed.");
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing {typeToConvert.Name}.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) {
        writer.WriteNumberValue(value ? trueNumber : falseNumber);
    }
}