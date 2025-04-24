using System.Diagnostics;

namespace WaveProxyAIO.Configurations {
    internal class CheckerStats {
        private readonly object _lock = new();

        public int ParsedProxies { get; set; }
        public int TotalProxies { get; set; }
        public int ValidProxies { get; set; }
        public int InvalidProxies { get; set; }
        public int TotalRetries { get; set; }

        public double InvalidProxiesPercentage => ParsedProxies > 0
            ? Math.Round((double)InvalidProxies / ParsedProxies * 100)
            : 0;

        public double ValidProxiesPercentage => ParsedProxies > 0
            ? Math.Round((double)ValidProxies / ParsedProxies * 100)
            : 0;

        public double ProxiesPerSecond => Runtime.TotalSeconds > 0
       ? Math.Round(ParsedProxies / Runtime.TotalSeconds)
       : 0;

        public double MemoryUsage => Math.Round(Process.GetCurrentProcess().PrivateMemorySize64 / Math.Pow(1024, 2));
        public TimeSpan Runtime => DateTime.Now - _start;
        public DateTime _start = DateTime.Now;

        public void Reset() {
            lock (_lock) {
                TotalProxies = 0;
                ParsedProxies = 0;
                ValidProxies = 0;
                InvalidProxies = 0;
                TotalRetries = 0;
                _start = DateTime.Now;
            }
        }
    }
}