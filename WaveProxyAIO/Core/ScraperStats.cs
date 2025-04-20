using System.Diagnostics;

namespace WaveProxyAIO.Core {
    internal class ScraperStats {
        public int TotalUrls { get; set; }
        public int ParsedUrls { get; set; }
        public int ValidUrls { get; set; }
        public int InvalidUrls => ParsedUrls - ValidUrls;
        public int UrlsPerSecond => (int)(ParsedUrls / Runtime.TotalSeconds);
        public int ValidUrlsPercentage => (int)((double)ValidUrls / ParsedUrls * 100);
        public int InvalidUrlsPercentage => (int)((double)InvalidUrls / ParsedUrls * 100);

        public int TotalProxies { get; set; }
        public int DuplicateCount { get; set; }
        public int ProxiesPerSite => TotalProxies / ParsedUrls;

        public double MemoryUsage => Math.Round(Process.GetCurrentProcess().PrivateMemorySize64 / Math.Pow(1024, 2));
        public TimeSpan Runtime => DateTime.Now - _start;
        public DateTime _start = DateTime.Now;

        //TODO: Reset start time for Runtime calculation
        public void Reset() {
            TotalUrls = 0;
            ParsedUrls = 0;
            ValidUrls = 0;
            DuplicateCount = 0;
            TotalProxies = 0;
        }
    }
}