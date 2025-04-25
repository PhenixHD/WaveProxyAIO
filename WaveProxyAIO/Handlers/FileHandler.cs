using System.Diagnostics;

namespace WaveProxyAIO.Handlers {
    internal class FileHandler {
        private readonly string _currentDirectory;
        private readonly string _urlFilePath;
        private readonly string _proxyFilePath;
        private readonly string _logFilePath;
        private readonly string _checkedProxiesFilePath;

        public FileHandler() {
            _currentDirectory = Path.GetDirectoryName(Environment.ProcessPath) ?? string.Empty;
            _urlFilePath = Path.Combine(_currentDirectory, "URLs.txt");
            _proxyFilePath = Path.Combine(_currentDirectory, "Proxies.txt");
            _logFilePath = Path.Combine(_currentDirectory, "Logs.txt");
            _checkedProxiesFilePath = Path.Combine(_currentDirectory, "CheckedProxies.txt");
        }

        public void CreateUrlFile() {
            Process.Start(new ProcessStartInfo {
                FileName = "explorer.exe",
                Arguments = _currentDirectory,
                UseShellExecute = true
            });

            File.Create(_urlFilePath).Dispose();
        }

        public void CreateProxyFile() {
            Process.Start(new ProcessStartInfo {
                FileName = "explorer.exe",
                Arguments = _currentDirectory,
                UseShellExecute = true
            });

            File.Create(_proxyFilePath).Dispose();
        }

        public bool CheckUrlFileExists() => File.Exists(_urlFilePath) ? true : false;

        public bool CheckProxyFileExists() => File.Exists(_proxyFilePath) ? true : false;

        public List<string> GetUrlsFromFile() => [.. File.ReadAllLines(_urlFilePath)];

        public List<string> GetProxiesFromFile() => [.. File.ReadAllLines(_proxyFilePath)];

        public void ClearProxyFile() => File.WriteAllText(_proxyFilePath, string.Empty);

        public void ClearCheckedProxyFile() => File.WriteAllText(_checkedProxiesFilePath, string.Empty);

        public void ClearLogFile() => File.WriteAllText(_logFilePath, string.Empty);

        public void AppendProxiesToFile(string[] proxyArray) => File.AppendAllLines(_proxyFilePath, proxyArray);

        public void AppendCheckedProxyToFile(string proxy) => File.AppendAllTextAsync(_checkedProxiesFilePath, $"{proxy}{Environment.NewLine}");

        public void AppendLogToFile(string logContent) => File.AppendAllTextAsync(_logFilePath, $"{DateTime.Now:HH:mm:ss}: {logContent}{Environment.NewLine}");

    }
}