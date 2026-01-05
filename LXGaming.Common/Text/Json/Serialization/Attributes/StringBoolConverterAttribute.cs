using System.Text.Json.Serialization;
using LXGaming.Common.Text.Json.Serialization.Converters;

namespace LXGaming.Common.Text.Json.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property
                | AttributeTargets.Field | AttributeTargets.Interface)]
public class StringBoolConverterAttribute(
    string falseString,
    string trueString,
    StringComparison comparisonType = StringComparison.Ordinal) : JsonConverterAttribute {

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert) {
        return new StringBoolConverter(falseString, trueString, comparisonType);
    }
}