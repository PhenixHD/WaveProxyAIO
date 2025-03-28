namespace WaveProxyAIO.UI {
    internal class ScraperStatus {

        public static void DisplayScraperStatus(int proxyCount, int urlCount, int urlProgress) {

            int currentLeft = Console.CursorLeft;
            int currentTop = Console.CursorTop;

            Console.SetCursorPosition(currentLeft, currentTop);
            Console.Write($"Loaded URLs: {urlCount}\n" +
                $"Progress: {urlProgress}/{urlCount}\n" +
                $"Proxies: {proxyCount}");

            Console.SetCursorPosition(currentLeft, currentTop);
        }

    }
}