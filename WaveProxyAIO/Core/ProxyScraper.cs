using Microsoft.Extensions.Configuration;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Core {
    internal class ProxyScraper(ProxyParser parser, MenuRenderer menuRenderer, IConfiguration config) {

        private readonly MenuRenderer _menuRenderer = menuRenderer ?? throw new ArgumentNullException(nameof(menuRenderer));
        private readonly ProxyParser _parser = parser ?? throw new ArgumentNullException(nameof(parser));
        private readonly bool _removeDupe = bool.Parse(config["Setting:RemoveDupe"] ?? "true");

        public async Task ScrapeProxies() {

            EmptyAllFiles();

            if (!Handlers.FileHandler.CheckUrlFileExists()) {

                _menuRenderer.ShowUrlFileMissing();

                Handlers.FileHandler.CreateUrlFile();

                ConsoleTextFormatter.PrintEmptyLine(4);
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            _menuRenderer.ShowScraperConfig();

            //TODO: Implement automatic duplicate removal - setting based on config
            //FIX: Move return to Main menu text to bottom -> Own method in MenuRenderer
            await _parser.ParseWebsite();

            ConsoleTextFormatter.PrintEmptyLine(4);
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

        private void EmptyAllFiles() {
            Handlers.FileHandler.ClearLogFile();
            Handlers.FileHandler.ClearProxyFile();
        }

    }
}