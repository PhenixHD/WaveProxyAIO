using System.Text.RegularExpressions;
using WaveProxyAIO.Helpers;
using WaveProxyAIO.Interfaces;

namespace WaveProxyAIO.Core {
    internal class ProxyParser(HttpClient client) : IProxyParser {
        private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));
        private readonly Regex _proxyRegex = RegexHelper.ProxyRegexPattern();
        private static readonly char[] _lineSeparators = ['\r', '\n'];

        public async Task<string[]> ParseWebsite(string url) {
            string rawData = await _client.GetStringAsync(url);
            string[] lines = rawData.Split(_lineSeparators, StringSplitOptions.RemoveEmptyEntries);

            List<string> proxies = [];
            foreach (string line in lines) {
                foreach (Match match in _proxyRegex.Matches(line)) {
                    proxies.Add(match.Value);
                }
            }

            return [.. proxies];
        }
    }
}