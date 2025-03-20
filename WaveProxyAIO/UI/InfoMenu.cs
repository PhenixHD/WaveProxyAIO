using Microsoft.Extensions.Configuration;

namespace WaveProxyAIO.UI {
    internal class InfoMenu {
        public static void DisplayInfo(GradientDesigner gradientDesigner, IConfiguration config) {
            Console.Clear();

            gradientDesigner.WriteGradient(AsciiDesigner.Info(), true);

            UITextFormatter.PrintEmptyLine(2);
            Console.WriteLine("Wave Proxy AIO - v1.0.0");
            Console.WriteLine("Last Update:\t07 March 2025");
            Console.WriteLine("Developer:\tPhenixHD");
            Console.WriteLine("Github: \thttps://github.com/PhenixHD");

            UITextFormatter.PrintEmptyLine(2);
            Console.WriteLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }
    }
}