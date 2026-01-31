using System.Globalization;
using System.Numerics;
using System.Text.Json.Serialization;
using LXGaming.Common.Text.Json.Serialization.Converters;

namespace LXGaming.Common.Text.Json.Serialization.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Property
                | AttributeTargets.Field | AttributeTargets.Interface)]
public class StringNumberConverterAttribute<T> : JsonConverterAttribute
    where T : struct, INumber<T> {

    private readonly NumberStyles? _style;

    public StringNumberConverterAttribute(NumberStyles style) {
        _style = style;
    }

    public StringNumberConverterAttribute() {
        // no-op
    }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert) {
        return new StringNumberConverter<T>(_style, null);
    }
}