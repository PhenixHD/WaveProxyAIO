using System.Drawing;

namespace WaveProxyAIO.Interfaces {

    internal interface IGradientStrategy {
        Color ColorRGB(int position, int totalLength);
    }
}