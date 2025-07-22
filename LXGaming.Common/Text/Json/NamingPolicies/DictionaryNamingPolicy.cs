using System.Text.Json;

namespace LXGaming.Common.Text.Json.NamingPolicies;

public class DictionaryNamingPolicy(IReadOnlyDictionary<string, string> dictionary) : JsonNamingPolicy {

    /// <inheritdoc />
    public override string ConvertName(string name) {
        return dictionary.GetValueOrDefault(name, name);
    }
}