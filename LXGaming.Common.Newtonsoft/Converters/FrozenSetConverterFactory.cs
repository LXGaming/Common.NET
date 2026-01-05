using System.Collections.Frozen;
using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

public class FrozenSetConverterFactory : JsonConverterFactory {

    /// <inheritdoc />
    public override bool CanConvert(Type objectType) {
        return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(FrozenSet<>);
    }

    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type objectType) {
        var genericArguments = objectType.GetGenericArguments();
        var itemType = genericArguments[0];
        var converterType = typeof(FrozenSetConverter<>).MakeGenericType(itemType);
        return (JsonConverter?) Activator.CreateInstance(converterType);
    }
}