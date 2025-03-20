using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WaveProxyAIO.Interfaces;
using WaveProxyAIO.Strategies;
using WaveProxyAIO.UI;

namespace WaveProxyAIO {
    internal class Program {
        private static void Main(string[] args) {

            Console.Title = "Wave AIO";
            Console.CursorVisible = false;

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            IServiceCollection service = new ServiceCollection();

            string? gradientType = config["GradientType"];

            if (gradientType == "Vertical") {
                service.AddSingleton<IGradientStrategy, VerticalGradientStrategy>();
            } else if (gradientType == "Horizontal") {
                service.AddSingleton<IGradientStrategy, HorizontalGradientStrategy>();
            } else {
                Console.WriteLine("[WARNING] Invalid or missing 'GradientType' in appsettings.json. Defaulting to VerticalGradientStrategy.");
                service.AddSingleton<IGradientStrategy, VerticalGradientStrategy>();
            }

            service.AddSingleton<IConfiguration>(config);
            service.AddSingleton<GradientDesigner>();

            var provider = service.BuildServiceProvider();
            var gradientdesigner = provider.GetService<GradientDesigner>();

            //Main Logic
            while (true) {
                UI.MainMenu.DisplayMenu(gradientdesigner);
                Handlers.MainMenuHandler.HandleUserInput(gradientdesigner, config);
            }

        }

    }
}