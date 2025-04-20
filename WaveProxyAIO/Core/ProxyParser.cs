using Microsoft.Extensions.Configuration;
using System.Text.RegularExpressions;
using WaveProxyAIO.Helpers;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Core {
    internal class ProxyParser(HttpClient client, SemaphoreSlim semaphore, MenuRenderer menuRenderer, ScraperStats scraperStats, IConfiguration config) {
        private readonly HttpClient _client = client ?? throw new ArgumentNullException(nameof(client));
        private readonly SemaphoreSlim _semaphore = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
        private readonly MenuRenderer _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));
        private readonly ScraperStats _scraperStats = scraperStats ?? throw new ArgumentNullException(nameof(scraperStats));
        private readonly bool _removeDupe = bool.Parse(config["Setting:RemoveDupe"] ?? "true");

        private static readonly char[] _lineSeparators = ['\r', '\n'];
        private static readonly object _lock = new();

        //TODO: Implement tracking of duplicate proxies
        public async Task ParseWebsite() {
            List<string> urls = Handlers.FileHandler.GetUrlsFromFile();
            HashSet<string> distinctProxies = new();

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
                            distinctProxies.Add(match.Value);
                        }
                    }

                    lock (_lock) {
                        if (_removeDupe) {
                            _scraperStats.TotalProxies += scrapedProxies.Count;
                            _scraperStats.DuplicateCount = _scraperStats.TotalProxies - distinctProxies.Count;
                        } else {
                            _scraperStats.TotalProxies += scrapedProxies.Count;
                            Handlers.FileHandler.AppendProxiesToFile([.. scrapedProxies]);
                        }
                    }

                    scrapedProxies.Clear();
                    _scraperStats.ValidUrls++;
                } catch (HttpRequestException e) {
                    Handlers.FileHandler.AppendLogToFile(e.Message);
                } catch (Exception e) {
                    Handlers.FileHandler.AppendLogToFile(e.Message);
                } finally {
                    lock (_lock) {
                        int currentLeft = Console.CursorLeft;
                        int currentTop = Console.CursorTop;
                        _scraperStats.ParsedUrls++;
                        _menuRenderer.ShowScraperStatus();
                        Console.SetCursorPosition(currentLeft, currentTop);
                        _semaphore.Release();
                    }
                }
            }));

            await Task.WhenAll(tasks);
            Handlers.FileHandler.WriteProxiesToFile([.. distinctProxies]);

            _menuRenderer.ShowScraperStatus();
        }
    }
}