using Microsoft.Extensions.Configuration;
using WaveProxyAIO.Handlers;
using WaveProxyAIO.Interfaces;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Core {
    internal class ProxyScraper {
        private readonly IProxyParser _parser;
        private readonly MenuRenderer _menuRenderer;
        private readonly ScraperStats _scraperStats;
        private readonly SemaphoreSlim _semaphore;
        private readonly IConfiguration _config;
        private readonly bool _removeDupe;
        private readonly object _lock = new object();

        public ProxyScraper(
            IProxyParser parser,
            MenuRenderer menuRenderer,
            ScraperStats scraperStats,
            SemaphoreSlim semaphore,
            IConfiguration config) {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));
            _scraperStats = scraperStats ?? throw new ArgumentNullException(nameof(scraperStats));
            _semaphore = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _removeDupe = bool.Parse(_config["Setting:RemoveDupe"] ?? "true");
        }

        public async Task ScrapeProxies() {
            EmptyAllFiles();
            _scraperStats.Reset();
            _scraperStats._start = DateTime.Now;

            if (!FileHandler.CheckUrlFileExists()) {
                _menuRenderer.ShowUrlFileMissing();
                FileHandler.CreateUrlFile();
                ConsoleTextFormatter.PrintEmptyLine(4);
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            _menuRenderer.ShowScraperConfig();

            List<string> urls = FileHandler.GetUrlsFromFile();
            HashSet<string> distinctProxies = [];
            _scraperStats.TotalUrls = urls.Count;

            IEnumerable<Task> tasks = urls.Select(async url => {
                await _semaphore.WaitAsync();
                try {
                    await ProcessUrlAsync(url, distinctProxies);
                } finally {
                    _semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);

            if (_removeDupe) {
                string[] uniqueProxies = [.. distinctProxies];
                _scraperStats.DuplicateCount = _scraperStats.TotalProxies - uniqueProxies.Length;
                FileHandler.WriteProxiesToFile(uniqueProxies);
            }

            _menuRenderer.ShowScraperStatus();
            ConsoleTextFormatter.PrintEmptyLine(2);
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private async Task ProcessUrlAsync(string url, HashSet<string> distinctProxies) {
            try {
                string[] scrapedProxies = await _parser.ParseWebsite(url);

                _scraperStats.TotalProxies += scrapedProxies.Length;

                if (_removeDupe) {
                    foreach (var proxy in scrapedProxies) {
                        distinctProxies.Add(proxy);
                    }
                } else {
                    FileHandler.AppendProxiesToFile(scrapedProxies);
                }

                _scraperStats.ValidUrls++;
            } catch (HttpRequestException e) {
                FileHandler.AppendLogToFile($"HttpRequestException for URL {url}: {e.Message}");
            } catch (Exception e) {
                FileHandler.AppendLogToFile($"General exception for URL {url}: {e.Message}");
            } finally {
                lock (_lock) {
                    int currentLeft = Console.CursorLeft;
                    int currentTop = Console.CursorTop;
                    _scraperStats.ParsedUrls++;
                    _menuRenderer.ShowScraperStatus();
                    Console.SetCursorPosition(currentLeft, currentTop);
                }
            }
        }

        private void EmptyAllFiles() {
            FileHandler.ClearLogFile();
            FileHandler.ClearProxyFile();
        }
    }
}