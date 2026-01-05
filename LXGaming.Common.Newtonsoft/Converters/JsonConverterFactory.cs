using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

public abstract class JsonConverterFactory : JsonConverter {

    public abstract JsonConverter? CreateConverter(Type objectType);

    protected JsonConverter CreateConverterInternal(Type objectType) {
        var converter = CreateConverter(objectType);
        if (converter == null) {
            throw new InvalidOperationException($"The converter '{GetType()}' cannot return a null value.");
        }

        if (converter is JsonConverterFactory) {
            throw new InvalidOperationException(
                $"The converter '{GetType()}' cannot return an instance of JsonConverterFactory.");
        }

        return converter;
    }

    /// <inheritdoc />
    public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
        if (value == null) {
            writer.WriteNull();
            return;
        }

        CreateConverterInternal(value.GetType()).WriteJson(writer, value, serializer);
    }

    /// <inheritdoc />
    public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue,
        JsonSerializer serializer) {
        if (reader.TokenType == JsonToken.Null) {
            return null;
        }

        return CreateConverterInternal(objectType).ReadJson(reader, objectType, existingValue, serializer);
    }
}