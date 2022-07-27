using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

[SuppressMessage("ReSharper", "IntroduceOptionalParameters.Global")]
public class StringBoolConverter : JsonConverter<bool> {

    private readonly string _falseKey;
    private readonly string _trueKey;
    private readonly StringComparison _stringComparison;

    public StringBoolConverter(string falseKey, string trueKey) : this(falseKey, trueKey, StringComparison.Ordinal) {
    }

    public StringBoolConverter(string falseKey, string trueKey, StringComparison stringComparison) {
        _falseKey = falseKey;
        _trueKey = trueKey;
        _stringComparison = stringComparison;
    }

    public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer) {
        writer.WriteValue(value ? _trueKey : _falseKey);
    }

    public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue, JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.String) {
            var value = Convert.ToString(reader.Value);
            if (string.Equals(value, _falseKey, _stringComparison)) {
                return false;
            }

            if (string.Equals(value, _trueKey, _stringComparison)) {
                return true;
            }

            throw new JsonSerializationException($"String value {value} is not allowed.");
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing {objectType.Name}.");
    }
}