using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;

namespace WaveProxyAIO.Core {
    internal class ProxyParser {
        private readonly HttpClient _client;
        private readonly SemaphoreSlim _semaphore;
        private static readonly object _lock = new object();

        public ProxyParser(IConfiguration config, HttpClient client, SemaphoreSlim semaphore) {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _semaphore = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
        }

        public async Task ParseWebsite() {
            List<string> urls = Handlers.FileHandler.GetURL();
            int urlCount = 0;

            Regex proxyRegex = new Regex(@"\b\d{1,3}(?:\.\d{1,3}){3}:(?:[0-5]?\d{1,4}|6[0-4]\d{3}|65[0-4]\d{2}|655[0-2]\d|6553[0-5])\b", RegexOptions.Compiled);
            List<string> validProxies = new List<string>();

            var tasks = urls.Select(async url => {
                await _semaphore.WaitAsync();
                try {
                    urlCount++;
                    string rawData = await _client.GetStringAsync(url);
                    string[] lines = rawData.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (string line in lines) {
                        MatchCollection matches = proxyRegex.Matches(line);

                        foreach (Match match in matches) {
                            validProxies.Add(match.Value);
                        }
                    }

                    lock (_lock) {
                        UpdateProxyCount(validProxies.Count, urls.Count, urlCount);
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

        private void UpdateProxyCount(int proxyCount, int urlCount, int urlProgress) {

            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;

            Console.SetCursorPosition(currentLeft, currentTop);
            Console.Write($"Loaded URLs: {urlCount}\n" +
                $"Progress: {urlProgress}/{urlCount}\n" +
                $"Proxies: {proxyCount}");

            Console.SetCursorPosition(currentLeft, currentTop);
        }
    }
}