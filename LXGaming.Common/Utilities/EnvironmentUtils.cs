using System.Diagnostics;

namespace LXGaming.Common.Utilities;

public static class EnvironmentUtils {

    public const string Development = "Development";
    public const string Staging = "Staging";
    public const string Production = "Production";

    public static bool IsDevelopment() {
        return IsEnvironment(Development);
    }

    public static bool IsStaging() {
        return IsEnvironment(Staging);
    }

    public static bool IsProduction() {
        return IsEnvironment(Production);
    }

    public static bool IsEnvironment(string environment) {
        return string.Equals(GetEnvironment(), environment, StringComparison.OrdinalIgnoreCase);
    }

    public static bool IsRunningInContainer() {
        return string.Equals(GetRunningInContainer(), "true", StringComparison.OrdinalIgnoreCase);
    }

    public static string GetEnvironment() {
        return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
               ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
               ?? Production;
    }

    public static string? GetRunningInContainer() {
        return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
    }

    public static DateTime GetStartTime() {
        using var process = Process.GetCurrentProcess();
        return process.StartTime;
    }
}