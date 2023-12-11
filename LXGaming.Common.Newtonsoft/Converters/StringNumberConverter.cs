﻿using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

public class StringNumberConverter<T> : JsonConverter<T?>
    where T : struct, IComparable, IConvertible, ISpanFormattable, IComparable<T>, IEquatable<T> {

    public override void WriteJson(JsonWriter writer, T? value, JsonSerializer serializer) {
        writer.WriteValue(value.ToString());
    }

    public override T? ReadJson(JsonReader reader, Type objectType, T? existingValue, bool hasExistingValue, JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Null) {
            return existingValue;
        }

        if (reader.TokenType == JsonToken.String) {
            return (T?) Convert.ChangeType(reader.Value, typeof(T));
        }

        throw new JsonSerializationException($"Unexpected token {reader.TokenType} when parsing {typeof(T).Name}.");
    }
}