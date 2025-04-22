using Leaf.xNet;
using Microsoft.Extensions.Configuration;

namespace WaveProxyAIO.Core {
    internal class ProxyChecker(IConfiguration config) {

        public async Task CheckProxies() {

            await TestProxy();
        }

        private async Task TestProxy() {
            HttpProxyClient proxy = HttpProxyClient.Parse("127.0.0.1:8080");

            using HttpRequest request = new();
            request.Proxy = proxy;
            request.ConnectTimeout = 5000;
            request.ReadWriteTimeout = 5000;

            string response = request.Get("https://api.ipify.org").ToString();
            Console.WriteLine("Proxy IP: " + response);
        }
    }
}