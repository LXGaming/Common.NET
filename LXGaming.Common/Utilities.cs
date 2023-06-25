using System.Reflection;

namespace LXGaming.Common;

public class Utilities {

    public static string GetAssemblyDescription(string assemblyString, string? packageName = null) {
        return GetAssemblyDescription(Assembly.Load(assemblyString), packageName ?? assemblyString);
    }

    public static string GetAssemblyDescription(Assembly assembly, string? packageName = null) {
        return $"{packageName ?? GetAssemblyName(assembly) ?? "null"} v{GetAssemblyVersion(assembly)}";
    }

    public static string? GetAssemblyName(Assembly assembly) {
        return assembly.GetName().Name;
    }

    public static string GetAssemblyVersion(Assembly assembly) {
        return (assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
                ?? assembly.GetCustomAttribute<AssemblyVersionAttribute>()?.Version
                ?? "null").Split('+', '-')[0];
    }

    public static bool IsRunningInContainer() {
        return string.Equals(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "true", StringComparison.OrdinalIgnoreCase);
    }
}