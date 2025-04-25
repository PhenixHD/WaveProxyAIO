using System.Diagnostics;

namespace WaveProxyAIO.Configurations {
    internal class CheckerStats {
        private readonly object _lock = new();

        public int TotalProxies { get; set; }
        public int CheckedProxies { get; set; }
        public int WorkingProxies { get; set; }
        public int NonWorkingProxies { get; set; }
        public int TotalRetryAttempts { get; set; }

        public double NonWorkingProxiesRate => CheckedProxies > 0
            ? Math.Round((double)NonWorkingProxies / CheckedProxies * 100)
            : 0;

        public double WorkingProxiesRate => CheckedProxies > 0
            ? Math.Round((double)WorkingProxies / CheckedProxies * 100)
            : 0;

        public double ProxiesCheckedPerSecond => SessionUptime.TotalSeconds > 0
       ? Math.Round(CheckedProxies / SessionUptime.TotalSeconds)
       : 0;

        public double MemoryUsage => Math.Round(Process.GetCurrentProcess().PrivateMemorySize64 / Math.Pow(1024, 2));
        public TimeSpan SessionUptime => DateTime.Now - SessionStart;
        public DateTime SessionStart = DateTime.Now;

        public void Reset() {
            lock (_lock) {
                TotalProxies = 0;
                CheckedProxies = 0;
                WorkingProxies = 0;
                NonWorkingProxies = 0;
                TotalRetryAttempts = 0;
                SessionStart = DateTime.Now;
            }
        }
    }
}