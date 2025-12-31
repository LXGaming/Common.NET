using System.Collections.Specialized;
using System.Text;
using System.Text.Encodings.Web;

namespace LXGaming.Common.Utilities;

public static class HttpUtils {

    public static string CreateQueryString(string key, string value) {
        var stringBuilder = new StringBuilder();
        AppendQueryParameter(stringBuilder, key, value);
        return stringBuilder.ToString();
    }

    public static string CreateQueryString(IEnumerable<KeyValuePair<string, string?>> parameters) {
        var stringBuilder = new StringBuilder();
        foreach (var parameter in parameters) {
            AppendQueryParameter(stringBuilder, parameter.Key, parameter.Value);
        }

        return stringBuilder.ToString();
    }

    public static string CreateQueryString(NameValueCollection parameters) {
        var stringBuilder = new StringBuilder();
        var keys = parameters.AllKeys;
        foreach (var key in keys) {
            var values = parameters.GetValues(key);
            if (values == null || values.Length == 0) {
                AppendQueryParameter(stringBuilder, key, null);
                continue;
            }

            foreach (var value in values) {
                AppendQueryParameter(stringBuilder, key, value);
            }
        }

        return stringBuilder.ToString();
    }

    public static void AppendQueryParameter(StringBuilder stringBuilder, string? key, string? value) {
        AppendQueryParameter(stringBuilder, key, value, stringBuilder.Length == 0);
    }

    public static void AppendQueryParameter(StringBuilder stringBuilder, string? key, string? value, bool start) {
        stringBuilder.Append(start ? '?' : '&');
        if (!string.IsNullOrEmpty(key)) {
            stringBuilder.Append(UrlEncoder.Default.Encode(key));
        }

        stringBuilder.Append('=');

        if (!string.IsNullOrEmpty(value)) {
            stringBuilder.Append(UrlEncoder.Default.Encode(value));
        }
    }
}