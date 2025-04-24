using System.Drawing;
using WaveProxyAIO.Interfaces;

namespace WaveProxyAIO.Handlers {
    internal class ColorGradientHandler : IColorGradient {
        public Color ColorRGB(int position, int totalLength, Color startColor, Color endColor) {
            int redDiff = endColor.R - startColor.R;
            int greenDiff = endColor.G - startColor.G;
            int blueDiff = endColor.B - startColor.B;

            float ratio = totalLength <= 1 ? 1f : (float)position / (totalLength - 1);
            int r = (int)(startColor.R + redDiff * ratio);
            int g = (int)(startColor.G + greenDiff * ratio);
            int b = (int)(startColor.B + blueDiff * ratio);

            return Color.FromArgb(r, g, b);
        }
    }
}