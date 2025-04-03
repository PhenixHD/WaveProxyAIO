using Microsoft.Extensions.Configuration;

namespace WaveProxyAIO.UI {
    internal class MenuRenderer(GradientDesigner gradientDesigner, IConfiguration config) {

        private readonly GradientDesigner _gradientDesigner = gradientDesigner ?? throw new ArgumentNullException(nameof(gradientDesigner));
        private readonly bool _removeDuplicateProxies = bool.Parse(config["Setting:RemoveDupe"] ?? "true");
        private readonly bool _checkProxiesAfterScraping = bool.Parse(config["Setting:CheckAfterScrape"] ?? "false");
        private readonly string _timeout = config["Setting:Timeout"] ?? "3000";
        private readonly string _threads = config["Setting:Threads"] ?? "20";

        public void ShowMainMenu() {
            Console.Clear();

            _gradientDesigner.DisplayGradient(AsciiDesigner.WaveAsciiArt(), true);
            ConsoleTextFormatter.PrintEmptyLine(4);
            _gradientDesigner.DisplayGradient(AsciiDesigner.MainMenuSelectionText());
        }

        public void ShowInfoMenu() {
            Console.Clear();

            _gradientDesigner.DisplayGradient(AsciiDesigner.InfoAsciiArt(), true);
            ConsoleTextFormatter.PrintEmptyLine(4);
            _gradientDesigner.DisplayGradient(AsciiDesigner.InfoText(), true);

            ConsoleTextFormatter.PrintEmptyLine(2);
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }

        public void ShowScraperMenu() {
            Console.Clear();

            _gradientDesigner.DisplayGradient(AsciiDesigner.ScraperAsciiArt(), true);
            ConsoleTextFormatter.PrintEmptyLine(2);
        }

        public void ShowScraperStatus(int proxyCount, int urlCount, int urlProgress) {

            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;

            Console.WriteLine("Section 1");
            Console.WriteLine("Progress | valids | invalids");
            Console.WriteLine("Section 2");
            Console.WriteLine("Current/Total | Avg/site");

            Console.SetCursorPosition(currentLeft, currentTop);
        }

        public void ShowScraperConfig() {
            string[] scraperOptions = [
                $"De-Dupe [{_removeDuplicateProxies}] | Auto-Check [{_checkProxiesAfterScraping}] | Timeout [{_timeout}ms] | Threads [{_threads}]",
                new string('_', Console.WindowWidth)
            ];

            _gradientDesigner.DisplayGradient(scraperOptions, true);
            ConsoleTextFormatter.PrintEmptyLine(2);
        }

        public void ShowUrlFileMissing() {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No URLs.txt found in running directory!");
            Console.WriteLine("Opening running directory..");
            Console.WriteLine("Please add URLs to URLs.txt..");
            Console.ResetColor();
        }
    }
}