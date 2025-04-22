using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using WaveProxyAIO.Handlers;
using WaveProxyAIO.Interfaces;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Core {
    internal class ProxyChecker {
        private readonly IProxyTester _proxyTester;
        private readonly FileHandler _filehandler;
        private readonly SemaphoreSlim _semaphore;
        private readonly MenuRenderer _menuRenderer;
        private readonly CheckerStats _checkerStats;
        private readonly string _host;
        private readonly int _timeout;
        private readonly object _lock = new();

        public ProxyChecker(IProxyTester proxyTester, IConfiguration config, SemaphoreSlim semaphore, FileHandler filehandler, MenuRenderer menuRenderer, CheckerStats checkerStats) {
            _proxyTester = proxyTester ?? throw new ArgumentNullException(nameof(proxyTester));
            _filehandler = filehandler ?? throw new ArgumentNullException(nameof(filehandler));
            _semaphore = semaphore ?? throw new ArgumentNullException(nameof(semaphore));
            _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));
            _checkerStats = checkerStats ?? throw new ArgumentNullException(nameof(checkerStats));
            _host = config["Setting:CheckProxySite"] ?? "https://google.com";
            _timeout = int.Parse(config["Setting:Timeout"] ?? "3000");
        }

        public async Task CheckProxies() {
            List<string> proxies = _filehandler.GetProxiesFromFile();
            _checkerStats.TotalProxies = proxies.Count;

            IEnumerable<Task> tasks = proxies.Select(async proxy => {
                await _semaphore.WaitAsync();
                try {
                    bool isValid = await _proxyTester.TestProxyAsync(proxy, _host, _timeout);
                    lock (_lock) {
                        if (isValid) {
                            _checkerStats.ValidProxies++;
                        } else {
                            _checkerStats.InvalidProxies++;
                        }
                        int currentLeft = Console.CursorLeft;
                        int currentTop = Console.CursorTop;
                        _checkerStats.ParsedProxies++;
                        _menuRenderer.ShowCheckerStatus();
                        Console.SetCursorPosition(currentLeft, currentTop);
                    }
                } finally {
                    _semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);

            Console.ReadKey();
        }
    }
}