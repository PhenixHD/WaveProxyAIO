using System.Net;
using WaveProxyAIO.Interfaces;

namespace WaveProxyAIO.Handlers {
    internal class Socks5ProxyHandler : IProxyTester {
        public async Task<bool> TestProxyAsync(string proxy, string host, int timeout) {
            SocketsHttpHandler handler = new() {
                Proxy = new WebProxy($"socks5://{proxy}"),
                ConnectTimeout = TimeSpan.FromMilliseconds(timeout),
                UseProxy = true
            };

            using HttpClient client = new HttpClient(handler) {
                Timeout = TimeSpan.FromMilliseconds(timeout)
            };

            try {
                var res = await client.GetAsync(host);
                return res.IsSuccessStatusCode;
            } catch (Exception) {
                return false;
            }
        }
    }
}