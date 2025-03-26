using System.Text.RegularExpressions;

namespace WaveProxyAIO.Handlers {
    internal class RegexHandler {
        public static Regex ProxyRegex() {
            return new Regex(@"\b\d{1,3}(?:\.\d{1,3}){3}:(?:[0-5]?\d{1,4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])\b", RegexOptions.Compiled);
        }
    }
}