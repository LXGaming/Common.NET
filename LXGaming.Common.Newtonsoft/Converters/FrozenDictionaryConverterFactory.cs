using System.Collections.Frozen;
using Newtonsoft.Json;

namespace LXGaming.Common.Newtonsoft.Converters;

public class FrozenDictionaryConverterFactory : JsonConverterFactory {

    /// <inheritdoc />
    public override bool CanConvert(Type objectType) {
        return objectType.IsGenericType && objectType.GetGenericTypeDefinition() == typeof(FrozenDictionary<,>);
    }

    /// <inheritdoc />
    public override JsonConverter? CreateConverter(Type objectType) {
        var genericArguments = objectType.GetGenericArguments();
        var keyType = genericArguments[0];
        var valueType = genericArguments[1];
        var converterType = typeof(FrozenDictionaryConverter<,>).MakeGenericType(keyType, valueType);
        return (JsonConverter?) Activator.CreateInstance(converterType);
    }
}