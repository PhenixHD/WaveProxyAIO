using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace WaveProxyAIO.Core {
    internal class ProxyParser {
        private readonly HttpClient _client;
        private readonly SemaphoreSlim _semaphore;
        private static readonly object _lock = new object();

        public ProxyParser(IConfiguration config, HttpClient client) {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _semaphore = new SemaphoreSlim(int.Parse(config["Setting:Thread"] ?? "10"));
        }

        public async Task ParseWebsite() {
            List<string> urls = Handlers.FileHandler.GetURL();

            Regex proxyRegex = new Regex(@"\b\d{1,3}(?:\.\d{1,3}){3}:(?:[0-5]?\d{1,4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])\b", RegexOptions.Compiled);
            List<string> validProxies = new List<string>();

            var tasks = urls.Select(async url => {
                await _semaphore.WaitAsync();
                try {
                    string rawData = await _client.GetStringAsync(url);
                    string[] lines = rawData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in lines) {
                        MatchCollection matches = proxyRegex.Matches(line);

                        foreach (Match match in matches) {
                            validProxies.Add(match.Value);
                        }
                    }

                    lock (_lock) {
                        UpdateProxyCount(validProxies.Count);
                    }

                } catch (HttpRequestException e) {
                    // Handle request error
                } catch (Exception e) {
                    // Handle unexpected error
                } finally {
                    _semaphore.Release();
                }
            }).ToList();

            await Task.WhenAll(tasks);

            Console.WriteLine("Done");
        }

        private void UpdateProxyCount(int count) {

            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;

            Console.SetCursorPosition(currentLeft, currentTop);
            Console.Write($"Proxies: {count}");

            Console.SetCursorPosition(currentLeft, currentTop);
        }
    }
}