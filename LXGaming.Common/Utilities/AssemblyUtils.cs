using System.Reflection;
using LXGaming.Common.Models;

namespace LXGaming.Common.Utilities;

public static class AssemblyUtils {

    public static string CreateDescription(string assemblyString, string? name = null) {
        return CreateDescription(Assembly.Load(assemblyString), name);
    }

    public static string CreateDescription(Assembly assembly, string? name = null) {
        return $"{name ?? assembly.GetName().Name ?? "Unknown"} v{GetVersion(assembly) ?? "Unknown"}";
    }

    public static InformationalVersion? GetInformationalVersion(Assembly assembly) {
        var value = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        return value != null ? InformationalVersion.Parse(value) : null;
    }

    public static string? GetVersion(Assembly assembly) {
        return GetInformationalVersion(assembly)?.Version
               ?? assembly.GetCustomAttribute<AssemblyFileVersionAttribute>()?.Version
               ?? assembly.GetName().Version?.ToString();
    }
}