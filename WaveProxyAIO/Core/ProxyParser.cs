using System.Text.RegularExpressions;
using WaveProxyAIO.Helpers;
using WaveProxyAIO.Interfaces;

namespace WaveProxyAIO.Core {
    internal class ProxyParser : IProxyParser {
        private readonly HttpClient _client;
        private readonly Regex _proxyRegex;
        private static readonly char[] _lineSeparators = ['\r', '\n'];

        public ProxyParser(HttpClient client) {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _proxyRegex = RegexHelper.ProxyRegexPattern();
        }

        public async Task<string[]> ParseWebsite(string url) {
            string rawData = await _client.GetStringAsync(url);
            string[] lines = rawData.Split(_lineSeparators, StringSplitOptions.RemoveEmptyEntries);

            List<string> proxies = new();
            foreach (string line in lines) {
                foreach (Match match in _proxyRegex.Matches(line)) {
                    proxies.Add(match.Value);
                }
            }

            return [.. proxies];
        }
    }
}
