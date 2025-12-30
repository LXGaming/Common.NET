using System.Reflection;
using System.Text.Json.Serialization.Metadata;
using LXGaming.Common.Text.Json.Serialization.Metadata.Modifiers;

namespace LXGaming.Common.Text.Json;

public static class Extensions {

    public static IJsonTypeInfoResolver WithModifier(this IJsonTypeInfoResolver resolver, IModifier modifier) {
        return resolver.WithAddedModifier(modifier.Execute);
    }

    public static IJsonTypeInfoResolver WithOrderPropertiesModifier(this IJsonTypeInfoResolver resolver) {
        return resolver.WithModifier(new OrderPropertiesModifier());
    }

    public static IJsonTypeInfoResolver WithRequiredPropertiesModifier(this IJsonTypeInfoResolver resolver,
        NullabilityInfoContext? nullabilityInfoContext = null) {
        return resolver.WithModifier(new RequiredPropertiesModifier(nullabilityInfoContext));
    }
}