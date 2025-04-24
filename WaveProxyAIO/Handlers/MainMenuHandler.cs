using WaveProxyAIO.Core;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Handlers {
    internal class MainMenuHandler(ProxyScraper scraper, ProxyChecker checker, MenuRenderer menuRenderer) {

        private readonly MenuRenderer _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));
        private readonly ProxyScraper _scraper = scraper ?? throw new ArgumentNullException(nameof(scraper));
        private readonly ProxyChecker _checker = checker ?? throw new ArgumentNullException(nameof(checker));

        public async Task HandleUserInput() {
            while (true) {
                char input = Console.ReadKey(true).KeyChar;

                switch (input) {
                    case '1':
                        _menuRenderer.ShowScraperMenu();
                        await _scraper.ScrapeProxies();
                        return;

                    case '2':
                        Console.Clear();
                        _menuRenderer.ShowCheckerMenu();
                        await _checker.CheckProxies();
                        return;

                    case '3':
                        Console.Clear();
                        _menuRenderer.ShowInfoMenu();
                        return;

                    case '9':
                        System.Environment.Exit(0);
                        break;

                    default:
                        break;
                }
            }
        }
    }
}