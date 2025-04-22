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
        private readonly object _lock = new();

        public async Task CheckProxies() {
            List<string> proxies = _filehandler.GetProxiesFromFile();
            _checkerStats.TotalProxies = proxies.Count;

            IEnumerable<Task> tasks = proxies.Select(async proxy => {
                await _semaphore.WaitAsync();
                string[] checkedProxies = [];
                try {
                    bool isValid = await _proxyTester.TestProxyAsync(proxy, _host, _timeout);
                    lock (_lock) {
                        if (isValid) {
                            _checkerStats.ValidProxies++;
                            _filehandler.AppendCheckedProxiesToFile(checkedProxies);
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