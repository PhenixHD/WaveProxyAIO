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

        public void ShowReturnMenu() {
            ConsoleTextFormatter.PrintEmptyLine(2);
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey(true);
        }

        public void ShowMainMenu() {
            string[] mainMenu = [
                    @"[1] Scrape Proxies",
                    @"[2] Check Proxies",
                    @"[3] Information",
                    @"[9] Exit"
            ];

            _gradientDesigner.DisplayGradient(AsciiDesigner.WaveAsciiArt(), true);
            ConsoleTextFormatter.PrintEmptyLine(4);
            _gradientDesigner.DisplayGradient(mainMenu);
        }

        public void ShowInfoMenu() {
            string[] infoMenu = [
                    @"This is a learning project",
                    @"",
                    @"Wave Proxy AIO [C#]",
                    @"Made with <3 by PhenixHD",
                    @"Github - https://github.com/PhenixHD"
            ];

            _gradientDesigner.DisplayGradient(AsciiDesigner.InfoAsciiArt(), true);
            ConsoleTextFormatter.PrintEmptyLine(4);
            _gradientDesigner.DisplayGradient(infoMenu, true);

            ShowReturnMenu();
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
                string[] scraperStatus = [
                    "[ URL Status ]",
                    $"Parsed      : {_scraperStats.ParsedUrls} / {_scraperStats.TotalUrls}  ",
                    $"Valid URLs  : {_scraperStats.ValidUrls} ({_scraperStats.ValidUrlsPercentage}%)    ",
                    $"Failed URLs : {_scraperStats.InvalidUrls} ({_scraperStats.InvalidUrlsPercentage}%)    ",
                    $"Retries     : {_scraperStats.TotalRetries}  ",
                    $"URLs/sec    : {_scraperStats.UrlsPerSecond}  ",
                    "",
                    "[ Proxy Stats ]",
                    $"Total found : {_scraperStats.TotalProxies}  ",
                    $"Duplicates  : {_scraperStats.DuplicateCount}  ",
                    $"Avg/Site    : {_scraperStats.ProxiesPerSite}  ",
                    "",
                    "[ Runtime ]",
                    $"Uptime      : {_scraperStats.Runtime:hh\\:mm\\:ss}  ",
                    $"Mem usage   : {_scraperStats.MemoryUsage} MB  "
                ];

                _gradientDesigner.DisplayGradient(scraperStatus);
            }
        }

        public void ShowCheckerStatus() {
            lock (_lock) {
                string[] checkerStatus = [
                    "[ Proxy Stats ]",
                    $"Progress    : {_checkerStats.ParsedProxies} / {_checkerStats.TotalProxies}  ",
                    $"Valid       : {_checkerStats.ValidProxies} ({_checkerStats.ValidProxiesPercentage}%)    ",
                    $"Invalid     : {_checkerStats.InvalidProxies} ({_checkerStats.InvalidProxiesPercentage}%)    ",
                    $"Retries     : {_checkerStats.TotalRetries}  ",
                    $"Proxies/sec : {_checkerStats.ProxiesPerSecond}  ",
                    "",
                    "[ Runtime ]",
                    $"Uptime      : {_checkerStats.Runtime:hh\\:mm\\:ss}  ",
                    $"Mem usage   : {_checkerStats.MemoryUsage} MB  "
                ];

                _gradientDesigner.DisplayGradient(checkerStatus);
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

        public void ShowProxyFileMissing() {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No Proxies.txt found in running directory!");
            Console.WriteLine("Opening running directory..");
            Console.WriteLine("Please add Proxies to Proxies.txt..");
            Console.ResetColor();
        }

    }
}