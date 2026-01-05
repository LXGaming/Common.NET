using System.Collections.Frozen;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Text.Json.Serialization.Converters;

public class FrozenSetConverterFactory : JsonConverterFactory {

    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert) {
        return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(FrozenSet<>);
    }

    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
        var genericArguments = typeToConvert.GetGenericArguments();
        var itemType = genericArguments[0];
        var converterType = typeof(FrozenSetConverter<>).MakeGenericType(itemType);
        return (JsonConverter?) Activator.CreateInstance(converterType);
    }
}