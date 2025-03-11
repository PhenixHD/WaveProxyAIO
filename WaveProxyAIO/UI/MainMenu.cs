namespace WaveProxyAIO.UI {
    internal class MainMenu {
        public static void DisplayMenu(GradientDesigner gradientDesigner) {
            Console.Clear();

            gradientDesigner.WriteGradient(AsciiDesigner.Wave(), true);

            UITextFormatter.PrintEmptyLine(4);

            Console.WriteLine(
                "[1] Scrape Proxies\n" +
                "[2] Check Proxies\n" +
                "[3] Info\n" +
                "[9] Exit");
        }
    }
}