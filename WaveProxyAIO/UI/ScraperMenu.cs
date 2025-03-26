using Microsoft.Extensions.Configuration;

namespace WaveProxyAIO.UI {
    internal class ScraperMenu {
        public static void DisplayScraper(GradientDesigner gradientDesigner, IConfiguration config) {
            Console.Clear();

            gradientDesigner.WriteGradient(AsciiDesigner.Scraper(), true);

            UITextFormatter.PrintEmptyLine(2);
        }

    }
}