using System.Text;

namespace LXGaming.Common.Models;

public record InformationalVersion {

    public required string Version { get; init; }

    public string? PreRelease { get; init; }

    public string? BuildMetadata { get; init; }

    public static InformationalVersion Parse(string value) {
        var minusIndex = value.IndexOf('-');
        var plusIndex = value.LastIndexOf('+');

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