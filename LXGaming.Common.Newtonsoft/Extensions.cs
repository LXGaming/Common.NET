using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LXGaming.Common.Newtonsoft;

public static class Extensions {

    public static T ToRequiredObject<T>(this JToken token) {
        return token.ToObject<T>() ?? throw new JsonException($"Failed to deserialize {typeof(T).Name}");
    }
}