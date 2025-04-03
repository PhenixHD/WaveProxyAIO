using System.Text.RegularExpressions;

namespace WaveProxyAIO.Helpers {
    internal static partial class RegexHelper {
        [GeneratedRegex(@"\b\d{1,3}(?:\.\d{1,3}){3}:(?:[0-5]?\d{1,4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])\b", RegexOptions.Compiled)]
        public static partial Regex ProxyRegexPattern();
    }
}