using System.Diagnostics;

namespace WaveProxyAIO.Configurations {
    internal class ScraperStats {
        private readonly object _lock = new();
        public int TotalUrls { get; set; }
        public int ParsedUrls { get; set; }
        public double ValidUrls { get; set; }
        public double InvalidUrls { get; set; }
        public int TotalRetries { get; set; }
        public double UrlsPerSecond => Runtime.TotalSeconds > 0
            ? Math.Round(ParsedUrls / Runtime.TotalSeconds)
            : 0;
        public double ValidUrlsPercentage => ParsedUrls > 0
            ? Math.Round(ValidUrls / ParsedUrls * 100)
            : 0;
        public double InvalidUrlsPercentage => ParsedUrls > 0
            ? Math.Round(InvalidUrls / ParsedUrls * 100)
            : 0;

        public int TotalProxies { get; set; }
        public int DuplicateCount { get; set; }
        public double ProxiesPerSite => ParsedUrls > 0 ? TotalProxies / ParsedUrls : 0;

        public double MemoryUsage => Math.Round(Process.GetCurrentProcess().PrivateMemorySize64 / Math.Pow(1024, 2));
        public TimeSpan Runtime => DateTime.Now - _start;
        public DateTime _start = DateTime.Now;

        //TODO: Reset start time for Runtime calculation
        public void Reset() {
            lock (_lock) {
                TotalUrls = 0;
                ParsedUrls = 0;
                ValidUrls = 0;
                InvalidUrls = 0;
                TotalProxies = 0;
                DuplicateCount = 0;
                _start = DateTime.Now;
            }
        }
    }
}