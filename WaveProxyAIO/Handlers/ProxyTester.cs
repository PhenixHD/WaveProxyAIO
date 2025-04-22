using WaveProxyAIO.Interfaces;

namespace WaveProxyAIO.Handlers {
    internal class ProxyTester : IProxyTester {

        public async Task<bool> TestProxyAsync(string proxy, string host, int timeout) {
            if (string.IsNullOrWhiteSpace(proxy)) throw new ArgumentException("Proxy cannot be null or empty.", nameof(proxy));
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentException("Host cannot be null or empty.", nameof(host));

            try {
                var httpClientHandler = new HttpClientHandler {
                    Proxy = new System.Net.WebProxy(proxy),
                    UseProxy = true
                };

                using var client = new HttpClient(httpClientHandler) {
                    Timeout = TimeSpan.FromMilliseconds(timeout)
                };

                var response = await client.GetAsync(host);
                return response.IsSuccessStatusCode;
            } catch {
                return false;
            }
        }
    }
}