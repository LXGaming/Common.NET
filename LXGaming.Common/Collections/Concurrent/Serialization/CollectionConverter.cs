﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Collections.Concurrent.Serialization;

public class CollectionConverter<TCollection, TItem> : JsonConverter<TCollection>
    where TCollection : ICollection<TItem> {

    /// <inheritdoc />
    public override TCollection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        if (reader.TokenType != JsonTokenType.StartArray) {
            throw new JsonException($"Unexpected token {reader.TokenType} when parsing '{typeToConvert.FullName}'.");
        }

        reader.Read();

        var items = Activator.CreateInstance<TCollection>();
        if (items == null) {
            throw new JsonException($"Failed to create an instance of '{typeof(TCollection).FullName}'.");
        }

        while (reader.TokenType != JsonTokenType.EndArray) {
            var item = JsonSerializer.Deserialize<TItem>(ref reader, options);
            if (item == null) {
                throw new JsonException($"Failed to deserialize '{typeof(TItem).FullName}'.");
            }

            items.Add(item);
            reader.Read();
        }

        return items;
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, TCollection value, JsonSerializerOptions options) {
        writer.WriteStartArray();

        foreach (var item in value) {
            JsonSerializer.Serialize(writer, item, options);
        }

        writer.WriteEndArray();
    }
}