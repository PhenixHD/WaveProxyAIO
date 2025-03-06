using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pastel;
using System.Drawing;
using WaveProxyAIO.UI;

namespace WaveProxyAIO {
    internal class Program {
        private static void Main(string[] args) {

            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            IServiceCollection service = new ServiceCollection();
            service.AddSingleton<IConfiguration>(config);
            service.AddSingleton<GradientDesigner>();

            var provider = service.BuildServiceProvider();
            var gradientdesigner = provider.GetService<GradientDesigner>();

            gradientdesigner.WriteGradient(AsciiDesigner.Wave(), true);
            gradientdesigner.WriteGradient(AsciiDesigner.Scraper(), true);
            gradientdesigner.WriteGradient(AsciiDesigner.Checker(), true);
            gradientdesigner.WriteGradient(AsciiDesigner.Info(), true);

        }

    }
}