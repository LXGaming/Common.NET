using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

[SuppressMessage("ReSharper", "IntroduceOptionalParameters.Global")]
public class StringBoolConverter(
    string falseString,
    string trueString,
    StringComparison comparisonType) : JsonConverter<bool> {

    public StringBoolConverter(string falseString, string trueString) : this(falseString, trueString,
        StringComparison.Ordinal) {
        // no-op
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer) {
        writer.WriteValue(value ? trueString : falseString);
    }

    /// <inheritdoc />
    public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue,
        JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.String) {
            var value = Convert.ToString(reader.Value);
            if (string.Equals(value, falseString, comparisonType)) {
                return false;
            }

            if (string.Equals(value, trueString, comparisonType)) {
                return true;
            }

            throw new JsonSerializationException($"String value '{value}' is not allowed.");
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing {objectType.Name}.");
    }
}