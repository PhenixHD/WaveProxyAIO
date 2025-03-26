using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using WaveProxyAIO.Core;
using WaveProxyAIO.Interfaces;
using WaveProxyAIO.Strategies;
using WaveProxyAIO.UI;

namespace WaveProxyAIO {
    internal class Program {
        private static async Task Main(string[] args) {
            //Practice project. 6th month programming.

            Console.Title = "Wave AIO";
            Console.CursorVisible = false;

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            IServiceCollection services = new ServiceCollection();

            string? gradientType = config["GradientType"];

            services.AddSingleton<IGradientStrategy>(provider => gradientType switch {
                "Horizontal" => new HorizontalGradientStrategy(),
                _ => new VerticalGradientStrategy()
            });

            HttpClient client = new HttpClient {
                Timeout = TimeSpan.FromMilliseconds(int.Parse(config["Setting:Timeout"] ?? "1000"))
            };

            SemaphoreSlim semaphore = new SemaphoreSlim(int.Parse(config["Setting:Threads"] ?? "10"));

            services.AddSingleton<HttpClient>(client);
            services.AddSingleton<SemaphoreSlim>(semaphore);
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<GradientDesigner>();
            services.AddSingleton<ProxyParser>();
            services.AddSingleton<ProxyScraper>();

            var serviceProvider = services.BuildServiceProvider();
            var gradientDesigner = serviceProvider.GetRequiredService<GradientDesigner>();
            var proxyScraper = serviceProvider.GetRequiredService<ProxyScraper>();

            // Main Logic
            while (true) {
                UI.MainMenu.DisplayMenu(gradientDesigner, config);
                await Handlers.MainMenuHandler.HandleUserInput(gradientDesigner, proxyScraper, config);
            }
        }
    }
}