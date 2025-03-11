namespace WaveProxyAIO.UI {
    internal class UITextFormatter {
        public static string CenterText(string text, bool isString = true) {
            int spaces = Math.Max(0, (Console.WindowWidth - text.Length) / 2);
            if (isString) {
                return text.PadLeft(spaces + text.Length);
            } else {
                return "".PadLeft(spaces);
            }

        }
    }
}