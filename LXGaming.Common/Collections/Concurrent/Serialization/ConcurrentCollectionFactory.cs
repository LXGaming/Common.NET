using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LXGaming.Common.Collections.Concurrent.Serialization;

public class ConcurrentCollectionFactory : JsonConverterFactory {

    public override bool CanConvert(Type typeToConvert) {
        return typeToConvert.IsGenericType && GetBaseTypes(typeToConvert)
            .Where(type => type.IsGenericType)
            .Any(type => type.GetGenericTypeDefinition() == typeof(ConcurrentCollection<,>));
    }

    public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
        var itemType = typeToConvert.GetGenericArguments()[0];
        return (JsonConverter?) Activator.CreateInstance(
            typeof(CollectionConverter<,>).MakeGenericType(typeToConvert, itemType),
            BindingFlags.Instance | BindingFlags.Public,
            null, null, null);
    }

    private IEnumerable<Type> GetBaseTypes(Type type) {
        var baseType = type.BaseType;
        while (baseType != null && baseType != typeof(object)) {
            yield return baseType;
            baseType = baseType.BaseType;
        }
    }
}