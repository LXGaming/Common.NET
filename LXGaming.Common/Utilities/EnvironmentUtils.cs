using System.Diagnostics;

namespace LXGaming.Common.Utilities;

public static class EnvironmentUtils {

    public static bool IsRunningInContainer() {
        return string.Equals(Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER"), "true", StringComparison.OrdinalIgnoreCase);
    }

    public static DateTime GetStartTime() {
        using var process = Process.GetCurrentProcess();
        return process.StartTime;
    }
}