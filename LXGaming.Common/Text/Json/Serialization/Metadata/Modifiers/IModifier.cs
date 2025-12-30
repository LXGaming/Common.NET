using System.Text.Json.Serialization.Metadata;

namespace LXGaming.Common.Text.Json.Serialization.Metadata.Modifiers;

public interface IModifier {

    void Execute(JsonTypeInfo jsonTypeInfo);
}