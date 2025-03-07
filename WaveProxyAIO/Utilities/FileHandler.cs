using System.Diagnostics;

namespace WaveProxyAIO.Utilities {
    internal class FileHandler {
        private static string currentDirectory = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);

        public static async Task SaveProxy(List<string> proxyList) {
            try {
                await File.WriteAllLinesAsync(Path.Combine(currentDirectory, "Proxy.txt"), proxyList);
            } catch {
                throw new Exception("Error saving files to Proxy.txt");
            }
        }

        public static async Task<string[]> ReadProxy(string filePath) {
            try {
                return await File.ReadAllLinesAsync(filePath);
            } catch {
                throw new FileLoadException("Error loading file!");
            }
        }
    }
}