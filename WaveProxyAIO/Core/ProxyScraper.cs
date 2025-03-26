using Microsoft.Extensions.Configuration;

namespace WaveProxyAIO.Core {
    internal class ProxyScraper {
        private readonly IConfiguration _config;
        private readonly ProxyParser _parser;
        public ProxyScraper(ProxyParser parser, IConfiguration config) {
            _config = config;
            _parser = parser;
        }

        //handles scraping
        public async Task Scrape() {
            if (!Handlers.FileHandler.ValidateURL()) {
                Handlers.FileHandler.CreateURL();

                UI.UITextFormatter.PrintEmptyLine(4);
                Console.WriteLine("Press any key to return...");
                Console.ReadKey();
                return;
            }

            await _parser.ParseWebsite();

            UI.UITextFormatter.PrintEmptyLine(4);
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

    }
}