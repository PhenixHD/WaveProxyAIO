namespace WaveProxyAIO.Core {
    internal class ProxyScraper {
        private readonly ProxyParser _parser;
        public ProxyScraper(ProxyParser parser) {
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

            string[] parsedProxies = await _parser.ParseWebsite();
            Handlers.FileHandler.SaveProxiesToFile(parsedProxies);

            UI.UITextFormatter.PrintEmptyLine(4);
            Console.WriteLine("Press any key to return...");
            Console.ReadKey();
        }

    }
}