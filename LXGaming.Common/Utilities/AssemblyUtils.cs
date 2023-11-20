using System.Reflection;

namespace LXGaming.Common.Utilities;

public static class AssemblyUtils {

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
}