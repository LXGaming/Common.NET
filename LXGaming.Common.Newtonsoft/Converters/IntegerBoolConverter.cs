using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

public class IntegerBoolConverter(int falseNumber, int trueNumber) : JsonConverter<bool> {

    public IntegerBoolConverter() : this(0, 1) {
        // no-op
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer) {
        writer.WriteValue(value ? trueNumber : falseNumber);
    }

    /// <inheritdoc />
    public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue,
        JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Integer) {
            var value = Convert.ToInt32(reader.Value);
            if (value == falseNumber) {
                return false;
            }

            if (value == trueNumber) {
                return true;
            }

            throw new JsonSerializationException($"Integer value '{value}' is not allowed.");
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing {objectType.Name}.");
    }
}