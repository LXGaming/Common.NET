using System.Globalization;
using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

public class DecimalConverter : JsonConverter<decimal> {

    public override void WriteJson(JsonWriter writer, decimal value, JsonSerializer serializer) {
        writer.WriteRawValue(value.ToString(CultureInfo.InvariantCulture));
    }

    public override decimal ReadJson(JsonReader reader, Type objectType, decimal existingValue, bool hasExistingValue, JsonSerializer serializer) {
        if (reader.TokenType is JsonToken.Float or JsonToken.Integer) {
            return Convert.ToDecimal(reader.Value);
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing {objectType.Name}.");
    }
}