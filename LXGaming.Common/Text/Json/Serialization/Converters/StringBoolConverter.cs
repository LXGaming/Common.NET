using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Text.Json.Serialization.Converters;

[SuppressMessage("ReSharper", "IntroduceOptionalParameters.Global")]
public class StringBoolConverter(
    string falseKey,
    string trueKey,
    StringComparison stringComparison) : JsonConverter<bool> {

    public StringBoolConverter(string falseKey, string trueKey) : this(falseKey, trueKey, StringComparison.Ordinal) {
        // no-op
    }

    /// <inheritdoc />
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType == JsonTokenType.String) {
            var value = reader.GetString();
            if (string.Equals(value, falseKey, stringComparison)) {
                return false;
            }

            if (string.Equals(value, trueKey, stringComparison)) {
                return true;
            }

            throw new JsonException($"String value '{value}' is not allowed.");
        }

        throw new JsonException($"Unexpected token {reader.TokenType} when parsing {typeToConvert.Name}.");
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options) {
        writer.WriteStringValue(value ? trueKey : falseKey);
    }
}