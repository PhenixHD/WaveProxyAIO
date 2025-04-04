using Microsoft.Extensions.Configuration;
using WaveProxyAIO.Core;

namespace WaveProxyAIO.UI {
    internal class MenuRenderer(GradientDesigner gradientDesigner, ScraperStats scraperStats, IConfiguration config) {

        private readonly GradientDesigner _gradientDesigner = gradientDesigner ?? throw new ArgumentNullException(nameof(gradientDesigner));
        private readonly ScraperStats _scraperStats = scraperStats ?? throw new ArgumentNullException(nameof(scraperStats));

        private readonly bool _removeDuplicateProxies = bool.Parse(config["Setting:RemoveDupe"] ?? "true");
        private readonly bool _checkProxiesAfterScraping = bool.Parse(config["Setting:CheckAfterScrape"] ?? "false");
        private readonly string _timeout = config["Setting:Timeout"] ?? "3000";
        private readonly string _threads = config["Setting:Threads"] ?? "20";

        private readonly object _lock = new();

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

        //TODO: Implement Stats from ScraperStatus
        public void ShowScraperStatus() {
            lock (_lock) {
                int currentLeft = Console.CursorLeft;
                int currentTop = Console.CursorTop;

                Console.WriteLine("[ URL Status ]");
                Console.WriteLine($"Parsed      : {_scraperStats.ParsedUrls} / {_scraperStats.TotalUrls}");
                Console.WriteLine($"Valid URLs  : {_scraperStats.ValidUrls} ({_scraperStats.ValidUrlsPercentage}%)");
                Console.WriteLine($"Failed URLs : {_scraperStats.InvalidUrls}  ({_scraperStats.InvalidUrlsPercentage}%)");
                Console.WriteLine($"URLs/sec    : {_scraperStats.UrlsPerSecond}");

                ConsoleTextFormatter.PrintEmptyLine(1);
                Console.WriteLine("[ Proxy Stats ]");
                Console.WriteLine($"Found       : {_scraperStats.TotalProxies}");
                Console.WriteLine($"Duplicates  : {_scraperStats.DuplicateCount}");
                Console.WriteLine($"Avg/Site    : {_scraperStats.ProxiesPerSite}");
                Console.WriteLine($"Proxies/sec : {_scraperStats.ProxiesPerSecond}");

                ConsoleTextFormatter.PrintEmptyLine(1);
                Console.WriteLine("[ Runtime ]");
                Console.WriteLine($"Uptime      : {_scraperStats.Runtime.Hours}h {_scraperStats.Runtime.Minutes}min {_scraperStats.Runtime.Seconds}s");
                Console.WriteLine($"ETA         : {_scraperStats.TimeRemaining.Hours}h {_scraperStats.TimeRemaining.Minutes}min {_scraperStats.TimeRemaining.Seconds}s");
                Console.WriteLine($"Mem usage   : {Math.Round(GC.GetTotalMemory(false) / Math.Pow(1024, 2))} MB");

                Console.SetCursorPosition(currentLeft, currentTop);
            }
        }

        public void ShowScraperConfig() {
            string[] scraperOptions = [
                $"De-Dupe [{_removeDuplicateProxies}] | Auto-Check [{_checkProxiesAfterScraping}] | Timeout [{_timeout}ms] | Threads [{_threads}]"
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