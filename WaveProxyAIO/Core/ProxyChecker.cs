using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using WaveProxyAIO.Handlers;
using WaveProxyAIO.Interfaces;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Core {
    internal class ProxyChecker(IProxyTester proxyTester, IConfiguration config, SemaphoreSlim semaphore, FileHandler filehandler, MenuRenderer menuRenderer, CheckerStats checkerStats) {
        private readonly IProxyTester _proxyTester = proxyTester ?? throw new ArgumentNullException(nameof(proxyTester));
        private readonly FileHandler _filehandler = filehandler ?? throw new ArgumentNullException(nameof(filehandler));
        private readonly SemaphoreSlim _semaphore = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
        private readonly MenuRenderer _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));
        private readonly CheckerStats _checkerStats = checkerStats ?? throw new ArgumentNullException(nameof(checkerStats));
        private readonly string _host = config["Setting:CheckProxySite"] ?? "https://google.com";
        private readonly int _timeout = int.Parse(config["Setting:ProxyTimeout"] ?? "3000");
        private readonly int _maxRetries = int.Parse(config["Setting:ProxyRetries"] ?? "2");
        private readonly object _lock = new();

        public async Task CheckProxies() {
            if (!PrepareChecking()) return;

            _menuRenderer.ShowCheckerConfig();

            List<string> proxies = _filehandler.GetProxiesFromFile();
            _checkerStats.TotalProxies = proxies.Count;

            await ProcessAllProxy(proxies);
            FinalizeChecking();
        }

        private async Task ProcessAllProxy(List<string> proxies) {
            IEnumerable<Task> tasks = proxies.Select(async proxy => {
                await _semaphore.WaitAsync();
                try {
                    await ProcessProxy(proxy);
                } finally {
                    lock (_lock) {
                        int currentLeft = Console.CursorLeft;
                        int currentTop = Console.CursorTop;
                        _checkerStats.ParsedProxies++;
                        _menuRenderer.ShowCheckerStatus();
                        Console.SetCursorPosition(currentLeft, currentTop);
                    }
                    _semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
        }

        private void FinalizeChecking() {
            _menuRenderer.ShowCheckerStatus();
            ConsoleTextFormatter.PrintEmptyLine(2);
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private async Task ProcessProxy(string proxy) {
            int attempt = 0;

            while (attempt < _maxRetries) {
                try {
                    bool isValid = await _proxyTester.TestProxyAsync(proxy, _host, _timeout);

                    if (isValid) {
                        _checkerStats.ValidProxies++;
                        _filehandler.AppendCheckedProxyToFile(proxy);
                        return;
                    }
                } finally {
                    attempt++;
                }

                if (attempt >= _maxRetries) {
                    lock (_lock) {
                        _checkerStats.InvalidProxies++;
                    }
                }

                _checkerStats.TotalRetries++;
            }
        }

        private bool PrepareChecking() {
            _checkerStats.Reset();

            if (!_filehandler.CheckProxyFileExists()) {
                _menuRenderer.ShowProxyFileMissing();
                _filehandler.CreateProxyFile();
                ConsoleTextFormatter.PrintEmptyLine(4);
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return false;
            }

            return true;
        }

    }
}