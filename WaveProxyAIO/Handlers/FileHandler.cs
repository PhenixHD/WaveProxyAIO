using System.Diagnostics;

namespace WaveProxyAIO.Handlers {
    internal class FileHandler {
        private static readonly string currentDirectory = Path.GetDirectoryName(Environment.ProcessPath) ?? string.Empty;
        private static readonly string urlFilePath = Path.Combine(currentDirectory, "URLs.txt");

        public static bool ValidateURL() {
            return File.Exists(urlFilePath);
        }

        public static void CreateURL() {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No URLs.txt found in running directory!");
            Console.WriteLine("Opening running directory..");
            Console.WriteLine("Please add URLs to URLs.txt..");
            Console.ResetColor();

            Process.Start(new ProcessStartInfo {
                FileName = "explorer.exe",
                Arguments = currentDirectory,
                UseShellExecute = true
            });
            File.Create(urlFilePath).Dispose();
        }

        public static List<string> GetURL() {
            return File.ReadAllLines(urlFilePath).ToList();
        }

    }
}