using Microsoft.Extensions.Configuration;
using WaveProxyAIO.Core;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Handlers {
    internal class MainMenuHandler {
        public static async Task HandleUserInput(GradientDesigner gradientDesigner, ProxyScraper scraper, IConfiguration config) {
            while (true) {
                char input = Console.ReadKey(true).KeyChar;

                switch (input) {
                    case '1':
                        Console.WriteLine("");
                        UI.ScraperMenu.DisplayScraper(gradientDesigner, config);
                        await scraper.Scrape();
                        return;

                    case '2':
                        //Placeholder - Call Checker Method once implemented + clear console
                        Console.WriteLine("");
                        return;

                    case '3':
                        Console.WriteLine("");
                        UI.InfoMenu.DisplayInfo(gradientDesigner, config);
                        return;

                    case '9':
                        System.Environment.Exit(0);
                        break;

                    default:
                        Console.WriteLine("");
                        UI.MainMenu.DisplayMenu(gradientDesigner, config);
                        break;
                }
            }
        }
    }
}