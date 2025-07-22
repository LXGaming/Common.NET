using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

[SuppressMessage("ReSharper", "IntroduceOptionalParameters.Global")]
public class StringBoolConverter(string falseKey, string trueKey, StringComparison stringComparison)
    : JsonConverter<bool> {

    public StringBoolConverter(string falseKey, string trueKey) : this(falseKey, trueKey, StringComparison.Ordinal) {
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer) {
        writer.WriteValue(value ? trueKey : falseKey);
    }

    /// <inheritdoc />
    public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue,
        JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.String) {
            var value = Convert.ToString(reader.Value);
            if (string.Equals(value, falseKey, stringComparison)) {
                return false;
            }

            if (string.Equals(value, trueKey, stringComparison)) {
                return true;
            }

            throw new JsonSerializationException($"String value '{value}' is not allowed.");
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing {objectType.Name}.");
    }
}