using System.Net;
using WaveProxyAIO.Interfaces;

namespace WaveProxyAIO.Handlers {
    internal class HttpProxyHandler : IProxyTester {
        public async Task<bool> TestProxyAsync(string proxy, string host, int timeout) {
            SocketsHttpHandler handler = new() {
                Proxy = new WebProxy($"https://{proxy}"),
                ConnectTimeout = TimeSpan.FromMilliseconds(timeout),
                UseProxy = true
            };

            using HttpClient client = new HttpClient(handler) {
                Timeout = TimeSpan.FromMilliseconds(timeout)
            };

            try {
                var res = await client.GetAsync(host);
                return res.IsSuccessStatusCode;
            } catch {
                return false;
            }
        }
    }
}