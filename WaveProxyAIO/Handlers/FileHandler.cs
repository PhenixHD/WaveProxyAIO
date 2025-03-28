using System.Diagnostics;

namespace WaveProxyAIO.Handlers {
    internal class FileHandler {
        private static readonly string _currentDirectory = Path.GetDirectoryName(Environment.ProcessPath) ?? string.Empty;
        private static readonly string _urlFilePath = Path.Combine(_currentDirectory, "URLs.txt");
        private static readonly string _proxyFilePath = Path.Combine(_currentDirectory, "Proxies.txt");

        public static bool ValidateURL() {
            return File.Exists(_urlFilePath);
        }

        public static void CreateURL() {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No URLs.txt found in running directory!");
            Console.WriteLine("Opening running directory..");
            Console.WriteLine("Please add URLs to URLs.txt..");
            Console.ResetColor();

            Process.Start(new ProcessStartInfo {
                FileName = "explorer.exe",
                Arguments = _currentDirectory,
                UseShellExecute = true
            });
            File.Create(_urlFilePath).Dispose();
        }

        public static List<string> GetURL() {
            return File.ReadAllLines(_urlFilePath).ToList();
        }

        public static void SaveProxiesToFile(string[] proxyArray) {
            File.WriteAllLinesAsync(_proxyFilePath, proxyArray);
        }

    }
}