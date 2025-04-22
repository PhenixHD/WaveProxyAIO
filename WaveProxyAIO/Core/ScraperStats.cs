using System.Diagnostics;

namespace WaveProxyAIO.Core {
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
        public double InvalidUrlsPercentage => Math.Round(InvalidUrls / ParsedUrls * 100);

        public int TotalProxies { get; set; }
        public int DuplicateCount { get; set; }
        public double ProxiesPerSite => TotalProxies / ParsedUrls;

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