using System.Diagnostics;

namespace WaveProxyAIO.Configurations {
    internal class ScraperStats {
        private readonly object _lock = new();

        public int TotalUrls { get; set; }
        public int ParsedUrls { get; set; }
        public double ValidUrlsCount { get; set; }
        public double InvalidUrlsCount { get; set; }
        public int TotalRetryAttempts { get; set; }

        public int TotalProxies { get; set; }
        public int DuplicateProxiesCount { get; set; }

        public double ValidUrlsPercentage => ParsedUrls > 0
            ? Math.Round(ValidUrlsCount / ParsedUrls * 100)
            : 0;
        public double InvalidUrlsPercentage => ParsedUrls > 0
            ? Math.Round(InvalidUrlsCount / ParsedUrls * 100)
            : 0;
        public double UrlsPerSecond => SessionUptime.TotalSeconds > 0
            ? Math.Round(ParsedUrls / SessionUptime.TotalSeconds)
            : 0;

        public double AverageProxiesPerUrl => ParsedUrls > 0 ? TotalProxies / ParsedUrls : 0;

        public double MemoryUsage => Math.Round(Process.GetCurrentProcess().PrivateMemorySize64 / Math.Pow(1024, 2));
        public TimeSpan SessionUptime => DateTime.Now - SessionStart;
        public DateTime SessionStart = DateTime.Now;

        //TODO: Reset start time for Runtime calculation
        public void Reset() {
            lock (_lock) {
                TotalUrls = 0;
                ParsedUrls = 0;
                ValidUrlsCount = 0;
                InvalidUrlsCount = 0;
                TotalProxies = 0;
                DuplicateProxiesCount = 0;
                SessionStart = DateTime.Now;
            }
        }
    }
}