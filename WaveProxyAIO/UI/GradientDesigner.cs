//Handles Color Rendering
using Microsoft.Extensions.Configuration;
using Pastel;
using System.Drawing;
using WaveProxyAIO.Interfaces;
using WaveProxyAIO.Strategies;

namespace WaveProxyAIO.UI {
    internal class GradientDesigner {

        private readonly IGradientStrategy _gradient;
        private readonly Color _startColor;
        private readonly Color _endColor;

        public GradientDesigner(IConfiguration config, IGradientStrategy gradient) {

            string? activeTheme = config["ActiveTheme"];
            if (string.IsNullOrEmpty(activeTheme)) {
                throw new ArgumentException("ActiveTheme is not configured properly.");
            }

            string? startColorHex = config[$"Themes:{activeTheme}:Start"];
            string? endColorHex = config[$"Themes:{activeTheme}:End"];
            if (string.IsNullOrEmpty(startColorHex) || string.IsNullOrEmpty(endColorHex)) {
                throw new ArgumentException("Theme colors are not configured properly.");
            }

            try {
                _startColor = ColorTranslator.FromHtml(startColorHex);
                _endColor = ColorTranslator.FromHtml(endColorHex);
            } catch (Exception ex) {
                throw new ArgumentException("Invalid color format in configuration.", ex);
            }

            _gradient = gradient;
        }

        //TODO: Refractor rendering of gradient
        public void DisplayGradient(string[] asciiArt, bool centerText = false) {

            if (asciiArt == null || asciiArt.Length == 0) {
                throw new ArgumentException("ASCII art cannot be null or empty.");
            }

            if (_gradient is HorizontalGradientStrategy) {
                foreach (string line in asciiArt) {
                    if (centerText)
                        Console.Write(ConsoleTextFormatter.CenterText(line, false));

                    for (int i = 0; i < line.Length; i++) {
                        Color charColor = _gradient.ColorRGB(i, line.Length, _startColor, _endColor);
                        Console.Write(line[i].ToString().Pastel(charColor));
                    }
                    Console.WriteLine();
                }
            } else {
                for (int i = 0; i < asciiArt.Length; i++) {
                    Color currentColor = _gradient.ColorRGB(i, asciiArt.Length, _startColor, _endColor);
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