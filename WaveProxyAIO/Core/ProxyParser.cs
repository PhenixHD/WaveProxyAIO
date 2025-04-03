using System.Text.RegularExpressions;
using WaveProxyAIO.Helpers;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Core {
    internal class ProxyParser(HttpClient client, SemaphoreSlim semaphore, MenuRenderer menuRenderer) {

        private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));
        private readonly SemaphoreSlim _semaphore = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
        private readonly MenuRenderer _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));

        private static readonly char[] _lineSeparators = ['\r', '\n'];
        private static readonly object _lock = new();

        public async Task ParseWebsite() {
            List<string> urls = Handlers.FileHandler.GetUrlsFromFile();
            int currentProgress = 0;

            Regex proxyRegex = RegexHelper.ProxyRegexPattern();

            var tasks = urls.Select(url => Task.Run(async () => {
                await _semaphore.WaitAsync();
                try {
                    string rawData = await _client.GetStringAsync(url);
                    string[] lines = rawData.Split(_lineSeparators, StringSplitOptions.RemoveEmptyEntries);

                    List<string> scrapedProxies = new();

                    foreach (string line in lines) {
                        MatchCollection matches = proxyRegex.Matches(line);

                        foreach (Match match in matches) {
                            scrapedProxies.Add(match.Value);
                        }
                    }

                    lock (_lock) {
                        Handlers.FileHandler.AppendProxiesToFile([.. scrapedProxies]);
                        _menuRenderer.ShowScraperStatus(scrapedProxies.Count, urls.Count, ++currentProgress);
                    }

                    scrapedProxies.Clear();

                } catch (HttpRequestException e) {
                    Handlers.FileHandler.AppendLogToFile(e.Message);
                } catch (Exception e) {
                    Handlers.FileHandler.AppendLogToFile(e.Message);
                } finally {
                    _semaphore.Release();
                }
            })).ToList();

            await Task.WhenAll(tasks);
        }
    }
}