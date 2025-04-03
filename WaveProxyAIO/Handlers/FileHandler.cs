using System.Diagnostics;

namespace WaveProxyAIO.Handlers {
    internal class FileHandler {
        private static readonly string _currentDirectory = Path.GetDirectoryName(Environment.ProcessPath) ?? string.Empty;
        private static readonly string _urlFilePath = Path.Combine(_currentDirectory, "URLs.txt");
        private static readonly string _proxyFilePath = Path.Combine(_currentDirectory, "Proxies.txt");
        private static readonly string _logFilePath = Path.Combine(_currentDirectory, "Logs.txt");

        public static bool CheckUrlFileExists() => File.Exists(_urlFilePath) ? true : false;

        public static void CreateUrlFile() {
            Process.Start(new ProcessStartInfo {
                FileName = "explorer.exe",
                Arguments = _currentDirectory,
                UseShellExecute = true
            });

            File.Create(_urlFilePath).Dispose();
        }

        public static List<string> GetUrlsFromFile() => File.ReadAllLines(_urlFilePath).ToList();

        public static void ClearProxyFile() => File.WriteAllText(_proxyFilePath, string.Empty);

        public static void AppendProxiesToFile(string[] proxyArray) => File.AppendAllLines(_proxyFilePath, proxyArray);

        public static void ClearLogFile() => File.WriteAllText(_logFilePath, string.Empty);

        public static void AppendLogToFile(string logContent) => File.AppendAllTextAsync(_logFilePath, $"{DateTime.Now:HH:mm:ss}: {logContent}{Environment.NewLine}");
    }
}