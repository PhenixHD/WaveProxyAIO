//
using Microsoft.Extensions.Configuration;
using Pastel;
using System.Drawing;

namespace WaveProxyAIO.UI {
    internal class GradientDesigner {

        private readonly Color _startColor;
        private readonly Color _endColor;

        public GradientDesigner(IConfiguration config) {
            // Get the selected theme from appsettings.json
            string activeTheme = config["ActiveTheme"];

            // Load the Start and End colors for the selected theme
            _startColor = ColorTranslator.FromHtml(config[$"Themes:{activeTheme}:Start"]);
            _endColor = ColorTranslator.FromHtml(config[$"Themes:{activeTheme}:End"]);
        }

        public void WriteGradient(string[] asciiArt, bool centerText = false) {

            for (int i = 0; i < asciiArt.Length; i++) {

                int spaces = (Console.WindowWidth - asciiArt[i].Length) / 2;

                float ratio = i / (float)(asciiArt.Length - 1);
                int r = (int)(_startColor.R + (_endColor.R - _startColor.R) * ratio);
                int g = (int)(_startColor.G + (_endColor.G - _startColor.G) * ratio);
                int b = (int)(_startColor.B + (_endColor.B - _startColor.B) * ratio);

                Color currentColor = Color.FromArgb(r, g, b);

                if (centerText) {
                    Console.WriteLine(asciiArt[i].PadLeft(spaces + asciiArt[i].Length).Pastel(currentColor));
                } else {
                    Console.WriteLine(asciiArt[i].Pastel(currentColor));
                }

            }
        }
    }
}