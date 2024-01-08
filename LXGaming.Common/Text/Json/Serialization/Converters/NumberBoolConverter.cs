using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Text.Json.Serialization.Converters;

public class NumberBoolConverter : JsonConverter<bool> {

    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.Number) {
            var value = reader.GetInt32();
            if (value is 0 or 1) {
                return value == 1;
            }

            throw new JsonException($"Integer value {value} is not allowed.");
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing {typeToConvert.Name}.");
    }

    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) {
        writer.WriteNumberValue(value ? 1 : 0);
    }
}