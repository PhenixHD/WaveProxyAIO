namespace WaveProxyAIO.Core {
    internal class ScraperStats {
        public int TotalUrls { get; set; }
        public int ParsedUrls { get; set; }
        public int ValidUrls { get; set; }
        public int ValidUrlsPercentage => (int)((double)ValidUrls / ParsedUrls * 100);
        public int InvalidUrls => ParsedUrls - ValidUrls;
        public int InvalidUrlsPercentage => (int)((double)InvalidUrls / ParsedUrls * 100);
        public int UrlsPerSecond => (int)(ParsedUrls / Runtime.TotalSeconds);
        public int TotalProxies { get; set; }
        public int DuplicateCount { get; set; }
        public int ProxiesPerSite => TotalProxies / ParsedUrls;
        public int ProxiesPerSecond => (int)(TotalProxies / Runtime.TotalSeconds);
        public TimeSpan Runtime => DateTime.Now - _start;
        public TimeSpan TimeRemaining => TimeSpan.FromSeconds((TotalUrls - ParsedUrls) * (Runtime.TotalSeconds / ParsedUrls));
        private readonly DateTime _start = DateTime.Now;

        //TODO: Implement Reset method to clear all stats
        public void Reset() {
            TotalUrls = 0;
            ParsedUrls = 0;
            ValidUrls = 0;
            DuplicateCount = 0;
            TotalProxies = 0;
        }
    }
}