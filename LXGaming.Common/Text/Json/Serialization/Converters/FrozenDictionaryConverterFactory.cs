using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Text.Json.Serialization.Converters;

public class FrozenDictionaryConverterFactory : JsonConverterFactory {

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) {
        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(FrozenDictionary<,>);
    }

    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
        var genericArguments = typeToConvert.GetGenericArguments();
        var keyType = genericArguments[0];
        var valueType = genericArguments[1];
        var converterType = typeof(FrozenDictionaryConverter<,>).MakeGenericType(keyType, valueType);
        return (JsonConverter?) Activator.CreateInstance(converterType);
    }
}