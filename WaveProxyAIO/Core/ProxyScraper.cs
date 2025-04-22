using Microsoft.Extensions.Configuration;
using WaveProxyAIO.Handlers;
using WaveProxyAIO.Interfaces;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Core {
    internal class ProxyScraper(IProxyParser parser, MenuRenderer menuRenderer, ScraperStats scraperStats, SemaphoreSlim semaphore, IConfiguration config, FileHandler filehandler) {
        private readonly IProxyParser _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        private readonly MenuRenderer _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));
        private readonly ScraperStats _scraperStats = scraperStats ?? throw new ArgumentNullException(nameof(scraperStats));
        private readonly SemaphoreSlim _semaphore = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
        private readonly FileHandler _filehandler = filehandler ?? throw new ArgumentException(nameof(filehandler));
        private readonly bool _removeDupe = bool.Parse(config["Setting:RemoveDupe"] ?? "true");
        private readonly int _maxRetries = int.Parse(config["Setting:Retries"] ?? "2");
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
            _scraperStats._start = DateTime.Now;

            if (!_filehandler.CheckUrlFileExists()) {
                _menuRenderer.ShowUrlFileMissing();
                _filehandler.CreateUrlFile();
                ConsoleTextFormatter.PrintEmptyLine(4);
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
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
                try {
                    await ParseUrl(url, distinctProxies);
                } finally {
                    lock (_lock) {
                        int currentLeft = Console.CursorLeft;
                        int currentTop = Console.CursorTop;
                        _scraperStats.ParsedUrls++;
                        _menuRenderer.ShowScraperStatus();
                        Console.SetCursorPosition(currentLeft, currentTop);
                    }
                    _semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
        }

        private async Task ParseUrl(string url, HashSet<string> distinctProxies) {
            int attempt = 0;

            while (attempt < _maxRetries) {
                try {
                    attempt++;
                    string[] scrapedProxies = await _parser.ParseWebsite(url);

                    _scraperStats.TotalProxies += scrapedProxies.Length;

                    if (_removeDupe) {
                        foreach (var proxy in scrapedProxies) {
                            distinctProxies.Add(proxy);
                        }
                    } else {
                        _filehandler.AppendProxiesToFile(scrapedProxies);
                    }

                    _scraperStats.ValidUrls++;
                    return;
                } catch (HttpRequestException e) {
                    _filehandler.AppendLogToFile($"HttpRequestException for URL {url}: {e.Message}");
                } catch (Exception e) {
                    _filehandler.AppendLogToFile($"General exception for URL {url}: {e.Message}");
                } finally {
                    _scraperStats.TotalRetries++;
                }

                if (attempt >= _maxRetries) {
                    lock (_lock) {
                        _scraperStats.InvalidUrls++;
                    }
                }
            }
        }

        private void FinalizeScraping(HashSet<string> distinctProxies) {
            if (_removeDupe) {
                string[] uniqueProxies = [.. distinctProxies];
                _scraperStats.DuplicateCount = _scraperStats.TotalProxies - uniqueProxies.Length;
                _filehandler.WriteProxiesToFile(uniqueProxies);
            }

            _menuRenderer.ShowScraperStatus();
            ConsoleTextFormatter.PrintEmptyLine(2);
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

    }
}