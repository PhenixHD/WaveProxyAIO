﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WaveProxyAIO.Configurations;
using WaveProxyAIO.Core;
using WaveProxyAIO.Handlers;
using WaveProxyAIO.Interfaces;
using WaveProxyAIO.UI;

namespace WaveProxyAIO {
    internal class Program {
        private static async Task Main() {
            //Practice project. 6th month programming.

            Console.Title = "Wave AIO";
            Console.CursorVisible = false;

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            IServiceCollection services = new ServiceCollection();

            string? proxyType = config["Setting:ProxyType"];
            services.AddSingleton<IProxyTester>(tester => proxyType switch {
                "SOCKS5" => new Socks5ProxyHandler(),
                "SOCKS4" => new Socks4ProxyHandler(),
                _ => new HttpProxyHandler()
            });

            HttpClient client = new() {
                Timeout = TimeSpan.FromMilliseconds(int.Parse(config["Setting:WebsiteTimeout"] ?? "3000"))
            };

            SemaphoreSlim semaphore = new(int.Parse(config["Setting:Threads"] ?? "50"));

            services.AddSingleton<HttpClient>(client);
            services.AddSingleton<SemaphoreSlim>(semaphore);
            services.AddSingleton<IConfiguration>(config);
            services.AddSingleton<IProxyParser, ProxyParserHandler>();
            services.AddSingleton<IColorGradient, ColorGradientHandler>();
            services.AddSingleton<GradientDesigner>();
            services.AddSingleton<SettingConfigurator>();
            services.AddSingleton<ProxyScraper>();
            services.AddSingleton<ProxyChecker>();
            services.AddSingleton<MenuRenderer>();
            services.AddSingleton<MainMenuHandler>();
            services.AddSingleton<ScraperStats>();
            services.AddSingleton<CheckerStats>();
            services.AddSingleton<FileHandler>();

            var serviceProvider = services.BuildServiceProvider();

            var menuRenderer = serviceProvider.GetRequiredService<MenuRenderer>();
            var mainMenuHandler = serviceProvider.GetRequiredService<MainMenuHandler>();

            // Main Logic
            while (true) {
                Console.Clear();
                menuRenderer.ShowMainMenu();
                await mainMenuHandler.HandleUserInput();
            }
        }
    }
}