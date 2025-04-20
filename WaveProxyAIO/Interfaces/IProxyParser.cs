namespace WaveProxyAIO.Interfaces {
    internal interface IProxyParser {
        Task<string[]> ParseWebsite(string url);
    }
}
