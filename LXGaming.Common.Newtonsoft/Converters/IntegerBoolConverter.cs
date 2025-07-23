﻿using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

public class IntegerBoolConverter : JsonConverter<bool> {

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, bool value, JsonSerializer serializer) {
        writer.WriteValue(value ? 1 : 0);
    }

    /// <inheritdoc />
    public override bool ReadJson(JsonReader reader, Type objectType, bool existingValue, bool hasExistingValue,
        JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Integer) {
            var value = Convert.ToInt32(reader.Value);
            if (value is 0 or 1) {
                return value == 1;
            }

            throw new JsonSerializationException($"Integer value '{value}' is not allowed.");
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing {objectType.Name}.");
    }
}