using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Text.Json.Serialization.Converters;

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
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.String) {
            var value = reader.GetString();
            if (string.Equals(value, falseString, comparisonType)) {
                return false;
            }

            if (string.Equals(value, trueString, comparisonType)) {
                return true;
            }

            throw new JsonException($"String value '{value}' is not allowed.");
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing {typeToConvert.Name}.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) {
        writer.WriteStringValue(value ? trueString : falseString);
    }
}