﻿using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace LXGaming.Common.Newtonsoft.Serialization;

public class ContractResolver : DefaultContractResolver {

    /// <summary>
    /// Gets or sets a value indicating whether the Required property is automatically set.
    /// </summary>
    public bool RequiredProperties { get; set; }

    private readonly NullabilityInfoContext _nullabilityInfoContext;

    public ContractResolver() {
        _nullabilityInfoContext = new NullabilityInfoContext();
    }

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

            property.Required = nullabilityInfo?.ReadState == NullabilityState.NotNull ? Required.Always : Required.Default;
        }

        return property;
    }
}