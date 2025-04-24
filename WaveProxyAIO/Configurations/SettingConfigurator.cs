using Microsoft.Extensions.Configuration;

namespace WaveProxyAIO.Configurations {
    internal class SettingConfigurator(IConfiguration config) {
        public int Threads { get; private set; } = int.Parse(config["Setting:Threads"] ?? "150");
        public int WebsiteRetries { get; private set; } = int.Parse(config["Setting:WebsiteRetries"] ?? "2");
        public int WebsiteTimeout { get; private set; } = int.Parse(config["Setting:WebsiteTimeout"] ?? "3000");
        public int ProxyRetries { get; private set; } = int.Parse(config["Setting:ProxyRetries"] ?? "2");
        public int ProxyTimeout { get; private set; } = int.Parse(config["Setting:ProxyTimeout"] ?? "3000");
        public bool RemoveDupe { get; private set; } = bool.Parse(config["Setting:RemoveDupe"] ?? "true");
        public string CheckProxySite { get; private set; } = config["Setting:CheckProxySite"] ?? "https://httpbin.org/ip";
    }
}