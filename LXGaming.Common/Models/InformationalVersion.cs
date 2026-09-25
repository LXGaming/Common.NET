using System.Text;
using LXGaming.Common.Utilities;

namespace LXGaming.Common.Models;

public record InformationalVersion {

    public required string Version { get; init; }

    public string? PreRelease { get; init; }

    public string? BuildMetadata { get; init; }

    public static InformationalVersion Parse(string value) {
        var minusIndex = value.IndexOf('-');
        var plusIndex = value.IndexOf('+');

        // Version
        if (minusIndex == -1 && plusIndex == -1) {
            return new InformationalVersion {
                Version = value
            };
        }

        // Version-PreRelease
        if (minusIndex != -1 && plusIndex == -1) {
            return new InformationalVersion {
                Version = value[..minusIndex],
                PreRelease = value[(minusIndex + 1)..]
            };
        }

        // Version+BuildMetadata
        if (minusIndex == -1 && plusIndex != -1) {
            return new InformationalVersion {
                Version = value[..plusIndex],
                BuildMetadata = value[(plusIndex + 1)..]
            };
        }

        // Version+BuildMetadata-PreRelease
        if (minusIndex > plusIndex) {
            // Everything after plus is BuildMetadata
            return new InformationalVersion {
                Version = value[..plusIndex],
                BuildMetadata = value[(plusIndex + 1)..]
            };
        }

        // Version-PreRelease+BuildMetadata
        return new InformationalVersion {
            Version = value[..minusIndex],
            PreRelease = value.Substring(minusIndex + 1, plusIndex - minusIndex - 1),
            BuildMetadata = value[(plusIndex + 1)..]
        };
    }

    public string? GetSourceRevisionId() {
        if (BuildMetadata == null) {
            return null;
        }

        if (StringUtils.IsGitRevisionId(BuildMetadata)) {
            return BuildMetadata;
        }

        // https://github.com/dotnet/sdk/blob/v8.0.100/src/Tasks/Microsoft.NET.Build.Tasks/targets/Microsoft.NET.GenerateAssemblyInfo.targets#L71
        var dotIndex = BuildMetadata.LastIndexOf('.');
        if (dotIndex == -1) {
            return null;
        }

        var value = BuildMetadata[(dotIndex + 1)..];
        if (StringUtils.IsGitRevisionId(value)) {
            return value;
        }

        return null;
    }

    /// <inheritdoc />
    public override string ToString() {
        var stringBuilder = new StringBuilder(Version);
        if (PreRelease != null) {
            stringBuilder.Append('-').Append(PreRelease);
        }

        if (BuildMetadata != null) {
            stringBuilder.Append('+').Append(BuildMetadata);
        }

        return stringBuilder.ToString();
    }
}