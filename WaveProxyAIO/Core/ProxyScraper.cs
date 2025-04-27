using Microsoft.Extensions.Configuration;
using WaveProxyAIO.Configurations;
using WaveProxyAIO.Handlers;
using WaveProxyAIO.Interfaces;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Core {
    internal class ProxyScraper(IProxyParser parser, MenuRenderer menuRenderer, ScraperStats scraperStats, SemaphoreSlim semaphore, FileHandler filehandler, SettingConfigurator settingConfigurator) {
        private readonly IProxyParser _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        private readonly MenuRenderer _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));
        private readonly ScraperStats _scraperStats = scraperStats ?? throw new ArgumentNullException(nameof(scraperStats));
        private readonly SemaphoreSlim _semaphore = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
        private readonly FileHandler _filehandler = filehandler ?? throw new ArgumentException(nameof(filehandler));
        private readonly bool _removeDupe = settingConfigurator.RemoveDupe;
        private readonly int _websiteRetries = settingConfigurator.WebsiteRetries;
        private readonly object _lock = new();

        public async Task ScrapeProxies() {
            if (!PrepareScraping()) return;

            _menuRenderer.ShowScraperConfig();

            List<string> urls = _filehandler.GetUrlsFromFile();
            HashSet<string> distinctProxies = [];
            _scraperStats.TotalUrls = urls.Count;

            await ProcessAllUrlsAsync(urls, distinctProxies);
            FinalizeScraping(distinctProxies);
        }

        private bool PrepareScraping() {
            EmptyAllFiles();
            _scraperStats.Reset();
            _scraperStats.SessionStart = DateTime.Now;

            if (!_filehandler.CheckUrlFileExists()) {
                _menuRenderer.ShowUrlFileMissing();
                _filehandler.CreateUrlFile();
                _menuRenderer.ShowReturnMenu();
                return false;
            }

            return true;
        }

        private void EmptyAllFiles() {
            _filehandler.ClearLogFile();
            _filehandler.ClearProxyFile();
        }

        private async Task ProcessAllUrlsAsync(List<string> urls, HashSet<string> distinctProxies) {
            IEnumerable<Task> tasks = urls.Select(async url => {
                await _semaphore.WaitAsync();
                bool isValid = false;
                try {
                    isValid = await ParseUrl(url, distinctProxies);
                } finally {
                    lock (_lock) {
                        int currentLeft = Console.CursorLeft;
                        int currentTop = Console.CursorTop;
                        _scraperStats.IncrementParsedUrl(isValid);
                        _menuRenderer.ShowScraperStatus();
                        Console.SetCursorPosition(currentLeft, currentTop);
                    }
                    _semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
        }

        private async Task<bool> ParseUrl(string url, HashSet<string> distinctProxies) {
            int attempt = 0;

            while (attempt <= _websiteRetries) {
                try {
                    attempt++;
                    string[] scrapedProxies = await _parser.ParseWebsite(url);

                    _scraperStats.TotalProxies += scrapedProxies.Length;

                    if (_removeDupe) {
                        foreach (string proxy in scrapedProxies) {
                            distinctProxies.Add(proxy);
                        }
                    } else {
                        _filehandler.AppendProxiesToFile(scrapedProxies);
                    }

                    return true;
                } catch (HttpRequestException e) {
                    _filehandler.AppendLogToFile($"HttpRequestException for URL {url}: {e.Message}");
                } catch (Exception e) {
                    _filehandler.AppendLogToFile($"General exception for URL {url}: {e.Message}");
                }
            }

            lock (_lock) {
                if (attempt > 1)
                    _scraperStats.TotalRetryAttempts += attempt - 1;
            }

            return false;
        }

        private void FinalizeScraping(HashSet<string> distinctProxies) {
            if (_removeDupe) {
                string[] uniqueProxies = [.. distinctProxies];
                _scraperStats.DuplicateProxiesCount = _scraperStats.TotalProxies - uniqueProxies.Length;
                _filehandler.AppendProxiesToFile(uniqueProxies);
            }

            _menuRenderer.ShowScraperStatus();
            _menuRenderer.ShowReturnMenu();
        }

    }
}