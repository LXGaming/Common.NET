using System.Collections.ObjectModel;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using LXGaming.Common.Text.Json.NamingPolicies;

namespace LXGaming.Common.Text.Json.Serialization.Converters;

public sealed class StringEnumConverter<TEnum>() : JsonStringEnumConverter<TEnum>(CreateNamingPolicy())
    where TEnum : struct, Enum {

    private static DictionaryNamingPolicy CreateNamingPolicy() {
        var fields = typeof(TEnum).GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        if (fields.Length == 0) {
            return new DictionaryNamingPolicy(ReadOnlyDictionary<string, string>.Empty);
        }

        var dictionary = new Dictionary<string, string>();
        foreach (var field in fields) {
            var jsonPropertyNameAttribute = field.GetCustomAttribute<JsonPropertyNameAttribute>(false);
            if (jsonPropertyNameAttribute != null) {
                dictionary.Add(field.Name, jsonPropertyNameAttribute.Name);
                continue;
            }

            var enumMemberAttribute = field.GetCustomAttribute<EnumMemberAttribute>(false);
            if (enumMemberAttribute?.Value != null) {
                dictionary.Add(field.Name, enumMemberAttribute.Value);
            }
        }

        return new DictionaryNamingPolicy(dictionary);
    }
}