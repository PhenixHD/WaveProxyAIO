namespace WaveProxyAIO.Interfaces {

    public interface IProxyTester {
        Task<bool> TestProxyAsync(string proxy, string host, int timeout);
    }
}