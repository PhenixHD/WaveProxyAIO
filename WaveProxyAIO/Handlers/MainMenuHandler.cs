using Microsoft.Extensions.Configuration;
using WaveProxyAIO.UI;

namespace WaveProxyAIO.Handlers {
    internal class MainMenuHandler {
        public static void HandleUserInput(GradientDesigner gradientDesigner, IConfiguration config) {
            while (true) {
                char input = Console.ReadKey(true).KeyChar;

                switch (input) {
                    case '1':
                        //Placeholder - Call Scraper Display Method once implemented + clear console
                        Console.WriteLine("Scrape");
                        return;

                    case '2':
                        //Placeholder - Call Checker Method once implemented + clear console
                        Console.WriteLine("Check");
                        return;

                    case '3':
                        //Placeholder - Call Info Display Method once implemented + clear console
                        Console.WriteLine("Info");
                        UI.InfoMenu.DisplayInfo(gradientDesigner, config);
                        return;

                    case '9':
                        //Exits Application
                        System.Environment.Exit(0);
                        break;

                    default:
                        //Placeholder - Re-Render Main menu + clear console
                        Console.WriteLine("");
                        UI.MainMenu.DisplayMenu(gradientDesigner);
                        break;
                }
            }
        }
    }
}