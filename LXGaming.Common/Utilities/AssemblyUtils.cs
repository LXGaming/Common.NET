using System.Reflection;

namespace LXGaming.Common.Utilities;

public static class AssemblyUtils {

    public static string GetDescription(string assemblyString, string? name = null) {
        return GetDescription(Assembly.Load(assemblyString), name);
    }

    public static string GetDescription(Assembly assembly, string? name = null) {
        return $"{name ?? GetName(assembly) ?? "null"} v{GetVersion(assembly) ?? "null"}";
    }

    public static string? GetName(Assembly assembly) {
        return assembly.GetName().Name;
    }

    public static string? GetVersion(Assembly assembly) {
        return (
            assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
            ?? assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version
            ?? assembly.GetName().Version?.ToString()
        )?.Split('+', '-')[0];
    }
}