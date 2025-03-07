using System.Drawing;
using WaveProxyAIO.Interfaces;

namespace WaveProxyAIO.Strategies {
    internal class VerticalGradientStrategy : IGradientStrategy {
        private Color _startColor;
        private Color _endColor;
        public VerticalGradientStrategy(Color startColor, Color endColor) {
            _startColor = startColor;
            _endColor = endColor;
        }

        public Color ColorRGB(int position, int totalLength) {
            int redDiff = _endColor.R - _startColor.R;
            int greenDiff = _endColor.G - _startColor.G;
            int blueDiff = _endColor.B - _startColor.B;

            float ratio = position / (totalLength - 1);
            int r = (int)(_startColor.R + redDiff * ratio);
            int g = (int)(_startColor.G + greenDiff * ratio);
            int b = (int)(_startColor.B + blueDiff * ratio);

            return Color.FromArgb(r, g, b);
        }
    }
}