using System.Text.RegularExpressions;
using WaveProxyAIO.Helpers;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Core {
    internal class ProxyParser(HttpClient client, SemaphoreSlim semaphore, MenuRenderer menuRenderer, ScraperStats scraperStats) {

        private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));
        private readonly SemaphoreSlim _semaphore = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
        private readonly MenuRenderer _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));
        private readonly ScraperStats _scraperStats = scraperStats ?? throw new ArgumentNullException(nameof(scraperStats));

        private static readonly char[] _lineSeparators = ['\r', '\n'];
        private static readonly object _lock = new();

        //TODO: Implement tracking of duplicate proxies
        public async Task ParseWebsite() {
            List<string> urls = Handlers.FileHandler.GetUrlsFromFile();

            _scraperStats.TotalUrls = urls.Count;

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

                    _scraperStats.TotalProxies += scrapedProxies.Count;

                    lock (_lock) {
                        Handlers.FileHandler.AppendProxiesToFile([.. scrapedProxies]);
                    }

                    scrapedProxies.Clear();
                    _scraperStats.ValidUrls++;
                } catch (HttpRequestException e) {
                    Handlers.FileHandler.AppendLogToFile(e.Message);
                } catch (Exception e) {
                    Handlers.FileHandler.AppendLogToFile(e.Message);
                } finally {
                    _scraperStats.ParsedUrls++;
                    _menuRenderer.ShowScraperStatus();
                    _semaphore.Release();
                }
            }));

            await Task.WhenAll(tasks);
        }
    }
}