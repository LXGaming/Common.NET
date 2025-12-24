using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;

namespace LXGaming.Common.Utilities;

public static class StringUtils {

    public static int CountMatches(string @string, string searchString,
        StringComparison stringComparison = StringComparison.Ordinal) {
        if (string.IsNullOrEmpty(@string) || string.IsNullOrEmpty(searchString)) {
            return 0;
        }

        var count = 0;
        var index = 0;
        while ((index = @string.IndexOf(searchString, index, stringComparison)) != -1) {
            count++;
            index += searchString.Length;
        }

        return count;
    }

    public static bool FixedTimeEquals(string left, string right, Encoding encoding) {
        var leftBytes = encoding.GetBytes(left);
        var rightBytes = encoding.GetBytes(right);
        return CryptographicOperations.FixedTimeEquals(leftBytes, rightBytes);
    }

    public static string GetEnumName(Enum @enum) {
        var name = @enum.ToString();
        var field = @enum.GetType().GetField(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
        var attribute = field?.GetCustomAttribute<EnumMemberAttribute>(false);
        return attribute?.Value ?? name;
    }

    public static string Join(char separator, params object?[] values) {
        return string.Join(separator, values
            .Select(value => value?.ToString())
            .Where(value => !string.IsNullOrEmpty(value)));
    }

    public static Version ParseVersion(string input) {
        return input.StartsWith('v') ? Version.Parse(input[1..]) : Version.Parse(input);
    }

    public static string ToHex(byte[] bytes) {
        var stringBuilder = new StringBuilder(bytes.Length * 2);
        foreach (var @byte in bytes) {
            stringBuilder.Append(@byte.ToString("x2"));
        }

        return stringBuilder.ToString();
    }
}