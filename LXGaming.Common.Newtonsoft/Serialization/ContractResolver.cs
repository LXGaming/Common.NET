﻿using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LXGaming.Common.Newtonsoft.Serialization;

public class ContractResolver(NullabilityInfoContext? nullabilityInfoContext = null) : DefaultContractResolver {

    /// <summary>
    /// Gets or sets a value indicating whether the Order property is automatically set.
    /// </summary>
    public bool OrderProperties { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the Required property is automatically set.
    /// </summary>
    public bool RequiredProperties { get; set; }

    private readonly NullabilityInfoContext _nullabilityInfoContext =
        nullabilityInfoContext ?? new NullabilityInfoContext();

    /// <inheritdoc />
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization) {
        var properties = base.CreateProperties(type, memberSerialization);

        if (OrderProperties) {
            var index = 0;
            foreach (var property in properties) {
                while (properties.Any(jsonProperty => jsonProperty.Order == index)) {
                    index++;
                }

                property.Order ??= index++;
            }

            return properties.OrderBy(property => property.Order ?? -1).ToList();
        }

        return properties;
    }

    /// <inheritdoc />
    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization) {
        var property = base.CreateProperty(member, memberSerialization);

        if (RequiredProperties) {
            NullabilityInfo? nullabilityInfo;
            lock (_nullabilityInfoContext) {
                nullabilityInfo = member switch {
                    EventInfo eventInfo => _nullabilityInfoContext.Create(eventInfo),
                    FieldInfo fieldInfo => _nullabilityInfoContext.Create(fieldInfo),
                    PropertyInfo propertyInfo => _nullabilityInfoContext.Create(propertyInfo),
                    _ => null
                };
            }

            var readState = nullabilityInfo?.ReadState;
            if (readState is NullabilityState.NotNull or NullabilityState.Nullable) {
                property.Required = readState == NullabilityState.NotNull ? Required.Always : Required.Default;
            }
        }

        return property;
    }
}