using WaveProxyAIO.Core;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Handlers {
    internal class MainMenuHandler(ProxyScraper scraper, MenuRenderer menuRenderer) {

        private readonly MenuRenderer _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));
        private readonly ProxyScraper _scraper = scraper ?? throw new ArgumentNullException(nameof(scraper));

        public async Task HandleUserInput() {
            while (true) {
                char input = Console.ReadKey(true).KeyChar;

                switch (input) {
                    case '1':
                        _menuRenderer.ShowScraperMenu();
                        await _scraper.ScrapeProxies();
                        return;

                    case '2':
                        //Placeholder - Call Checker Method once implemented + clear console
                        Console.WriteLine("");
                        return;

                    case '3':
                        _menuRenderer.ShowInfoMenu();
                        return;

                    case '9':
                        System.Environment.Exit(0);
                        break;

                    default:
                        _menuRenderer.ShowMainMenu();
                        break;
                }
            }
        }
    }
}