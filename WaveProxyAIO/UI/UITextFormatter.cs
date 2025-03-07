namespace WaveProxyAIO.UI {
    internal class UITextFormatter {
        public static string CenterText(string text) {
            int spaces = Math.Max(0, (Console.WindowWidth - text.Length) / 2);
            return text.PadLeft(spaces + text.Length);
        }
    }
}