using System.Reflection;
using System.Text.Json.Serialization.Metadata;

namespace LXGaming.Common.Text.Json.Serialization.Metadata.Modifiers;

/// <summary>
/// Automatically sets the Required property.
/// </summary>
public class RequiredPropertiesModifier(NullabilityInfoContext? nullabilityInfoContext = null) : IModifier {

    private readonly NullabilityInfoContext _nullabilityInfoContext =
        nullabilityInfoContext ?? new NullabilityInfoContext();

    public void Execute(JsonTypeInfo jsonTypeInfo) {
        if (jsonTypeInfo.Kind != JsonTypeInfoKind.Object) {
            return;
        }

        var properties = jsonTypeInfo.Properties;
        if (properties.Count == 0) {
            return;
        }

        foreach (var property in properties) {
            var attributeProvider = property.AttributeProvider;
            if (attributeProvider == null) {
                continue;
            }

            NullabilityInfo? nullabilityInfo;
            lock (_nullabilityInfoContext) {
                nullabilityInfo = attributeProvider switch {
                    EventInfo eventInfo => _nullabilityInfoContext.Create(eventInfo),
                    FieldInfo fieldInfo => _nullabilityInfoContext.Create(fieldInfo),
                    ParameterInfo parameterInfo => _nullabilityInfoContext.Create(parameterInfo),
                    PropertyInfo propertyInfo => _nullabilityInfoContext.Create(propertyInfo),
                    _ => null
                };
            }

            var readState = nullabilityInfo?.ReadState;
            if (readState is NullabilityState.NotNull or NullabilityState.Nullable) {
                property.IsRequired = readState == NullabilityState.NotNull;
            }
        }
    }
}