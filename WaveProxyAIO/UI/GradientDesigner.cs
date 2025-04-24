//Handles Color Rendering
using Microsoft.Extensions.Configuration;
using Pastel;
using System.Drawing;
using WaveProxyAIO.Interfaces;

namespace WaveProxyAIO.UI {
    internal class GradientDesigner {

        private readonly IColorGradient _colorGradient;
        private readonly string _gradientType;
        private readonly string _activeTheme;
        private readonly Color _startColor;
        private readonly Color _endColor;

        public GradientDesigner(IConfiguration config, IColorGradient colorGradient) {
            _colorGradient = colorGradient ?? throw new ArgumentNullException(nameof(colorGradient));
            _gradientType = config["GradientType"] ?? throw new ArgumentNullException(nameof(config));
            _activeTheme = config["ActiveTheme"] ?? throw new ArgumentNullException(nameof(config));
            _startColor = ColorTranslator.FromHtml(config[$"Themes:{_activeTheme}:Start"] ?? "#FFFFFF");
            _endColor = ColorTranslator.FromHtml(config[$"Themes:{_activeTheme}:End"] ?? "#FFFFFF");
        }

        public void DisplayGradient(string[] asciiArt, bool centerText = false) {

            if (asciiArt == null || asciiArt.Length == 0) return;

            if (_gradientType == "Horizontal") {
                foreach (string line in asciiArt) {
                    if (centerText)
                        Console.Write(ConsoleTextFormatter.CenterText(line, false));

                    for (int i = 0; i < line.Length; i++) {
                        Color charColor = _colorGradient.ColorRGB(i, line.Length, _startColor, _endColor);
                        Console.Write(line[i].ToString().Pastel(charColor));
                    }
                    Console.WriteLine();
                }
            } else {
                for (int i = 0; i < asciiArt.Length; i++) {
                    Color currentColor = _colorGradient.ColorRGB(i, asciiArt.Length, _startColor, _endColor);
                    if (centerText) {
                        Console.WriteLine(ConsoleTextFormatter.CenterText(asciiArt[i]).Pastel(currentColor));
                    } else {
                        Console.WriteLine(asciiArt[i].Pastel(currentColor));
                    }
                }
            }
        }
    }
}