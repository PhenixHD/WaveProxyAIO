using System.Drawing;

namespace WaveProxyAIO.Interfaces {

    internal interface IColorGradient {
        Color ColorRGB(int position, int totalLength, Color startColor, Color endColor);
    }
}