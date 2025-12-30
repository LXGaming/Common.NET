using System.Text.Json.Serialization.Metadata;

namespace LXGaming.Common.Text.Json.Serialization.Metadata.Modifiers;

/// <summary>
/// Automatically sets the Order property.
/// </summary>
public class OrderPropertiesModifier : IModifier {

    public void Execute(JsonTypeInfo jsonTypeInfo) {
        if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object) {
            return;
        }

        var properties = jsonTypeInfo.Properties;
        if (properties.Count == 0) {
            return;
        }

        var index = 1;
        foreach (var property in properties) {
            while (properties.Any(info => info.Order == index)) {
                index++;
            }

            if (property.Order == 0) {
                property.Order = index++;
            }
        }
    }
}