using Microsoft.Extensions.Configuration;
using WaveProxyAIO.Core;

namespace WaveProxyAIO.UI {
    internal class MenuRenderer(GradientDesigner gradientDesigner, ScraperStats scraperStats, CheckerStats checkerStats, IConfiguration config) {

        private readonly GradientDesigner _gradientDesigner = gradientDesigner ?? throw new ArgumentNullException(nameof(gradientDesigner));
        private readonly ScraperStats _scraperStats = scraperStats ?? throw new ArgumentNullException(nameof(scraperStats));
        private readonly CheckerStats _checkerStats = checkerStats ?? throw new ArgumentNullException(nameof(checkerStats));

        private readonly bool _removeDuplicateProxies = bool.Parse(config["Setting:RemoveDupe"] ?? "true");
        private readonly string _websiteTimeout = config["Setting:WebsiteTimeout"] ?? "3000";
        private readonly string _proxyTimeout = config["Setting:ProxyTimeout"] ?? "3000";
        private readonly string _threads = config["Setting:Threads"] ?? "50";
        private readonly string _websiteRetries = config["Setting:WebsiteRetries"] ?? "2";
        private readonly string _proxyRetries = config["Setting:ProxyRetries"] ?? "2";

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

        public void ShowCheckerMenu() {
            Console.Clear();

            _gradientDesigner.DisplayGradient(AsciiDesigner.CheckerAsciiArt(), true);
            ConsoleTextFormatter.PrintEmptyLine(2);
        }

        public void ShowScraperStatus() {
            lock (_lock) {
                Console.WriteLine("[ URL Status ]");
                Console.WriteLine($"Parsed      : {_scraperStats.ParsedUrls} / {_scraperStats.TotalUrls}  ");
                Console.WriteLine($"Valid URLs  : {_scraperStats.ValidUrls} ({_scraperStats.ValidUrlsPercentage}%)    ");
                Console.WriteLine($"Failed URLs : {_scraperStats.InvalidUrls} ({_scraperStats.InvalidUrlsPercentage}%)    ");
                Console.WriteLine($"Retries     : {_scraperStats.TotalRetries}  ");
                Console.WriteLine($"URLs/sec    : {_scraperStats.UrlsPerSecond}  ");

                ConsoleTextFormatter.PrintEmptyLine(1);
                Console.WriteLine("[ Proxy Stats ]");
                Console.WriteLine($"Total found : {_scraperStats.TotalProxies}  ");
                Console.WriteLine($"Duplicates  : {_scraperStats.DuplicateCount}  ");
                Console.WriteLine($"Avg/Site    : {_scraperStats.ProxiesPerSite}  ");

                ConsoleTextFormatter.PrintEmptyLine(1);
                Console.WriteLine("[ Runtime ]");
                Console.WriteLine($"Uptime      : {_scraperStats.Runtime:hh\\:mm\\:ss}  ");
                Console.WriteLine($"Mem usage   : {_scraperStats.MemoryUsage} MB  ");
            }
        }

        public void ShowCheckerStatus() {
            lock (_lock) {
                Console.WriteLine("[ Proxy Stats ]");
                Console.WriteLine($"Progress    : {_checkerStats.ParsedProxies} / {_checkerStats.TotalProxies}  ");
                Console.WriteLine($"Valid       : {_checkerStats.ValidProxies} ({_checkerStats.ValidProxiesPercentage}%)    ");
                Console.WriteLine($"Invalid     : {_checkerStats.InvalidProxies} ({_checkerStats.InvalidProxiesPercentage}%)    ");
                Console.WriteLine($"Retries     : {_checkerStats.TotalRetries}  ");
                Console.WriteLine($"Proxies/sec : {_checkerStats.ProxiesPerSecond}  ");

                ConsoleTextFormatter.PrintEmptyLine(1);
                Console.WriteLine("[ Runtime ]");
                Console.WriteLine($"Uptime      : {_checkerStats.Runtime:hh\\:mm\\:ss}  ");
                Console.WriteLine($"Mem usage   : {_checkerStats.MemoryUsage} MB  ");
            }
        }

        public void ShowScraperConfig() {
            string[] scraperOptions = [
                $"De-Dupe [{_removeDuplicateProxies}] | " +
                $"Timeout [{_websiteTimeout}ms] | " +
                $"Threads [{_threads}] | " +
                $"Retries [{_websiteRetries}]"
            ];

            _gradientDesigner.DisplayGradient(scraperOptions, true);
            ConsoleTextFormatter.PrintEmptyLine(2);
        }

        public void ShowCheckerConfig() {
            string[] checkerOptions = [
                $"De-Dupe [{_removeDuplicateProxies}] | " +
                $"Timeout [{_proxyTimeout}ms] | " +
                $"Threads [{_threads}] | " +
                $"Retries [{_proxyRetries}]"
            ];

            _gradientDesigner.DisplayGradient(checkerOptions, true);
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