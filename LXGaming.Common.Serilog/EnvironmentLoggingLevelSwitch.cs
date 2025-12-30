using Serilog.Core;
using Serilog.Events;

namespace LXGaming.Common.Serilog;

public class EnvironmentLoggingLevelSwitch : LoggingLevelSwitch {

    public EnvironmentLoggingLevelSwitch(LogEventLevel developmentLevel, LogEventLevel stagingLevel,
        LogEventLevel productionLevel) : base(productionLevel) {
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                          ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
        if (string.Equals(environment, "Development", StringComparison.OrdinalIgnoreCase)) {
            MinimumLevel = developmentLevel;
        } else if (string.Equals(environment, "Staging", StringComparison.OrdinalIgnoreCase)) {
            MinimumLevel = stagingLevel;
        }
    }

    public EnvironmentLoggingLevelSwitch(LogEventLevel developmentLevel = LogEventLevel.Debug,
        LogEventLevel productionLevel = LogEventLevel.Information)
        : this(developmentLevel, productionLevel, productionLevel) {
        // no-op
    }
}