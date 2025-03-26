using Microsoft.Extensions.Configuration;

namespace WaveProxyAIO.UI {
    internal class MainMenu {
        public static void DisplayMenu(GradientDesigner gradientDesigner, IConfiguration config) {
            Console.Clear();

            gradientDesigner.WriteGradient(AsciiDesigner.Wave(), true);

            UITextFormatter.PrintEmptyLine(4);

            gradientDesigner.WriteGradient(AsciiDesigner.MainMenu());
        }
    }
}