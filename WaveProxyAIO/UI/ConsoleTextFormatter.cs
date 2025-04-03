namespace WaveProxyAIO.UI {
    internal class ConsoleTextFormatter {
        public static string CenterText(string text, bool isString = true) {
            int spaces = Math.Max(0, (Console.WindowWidth - text.Length) / 2);
            if (isString) {
                return text.PadLeft(spaces + text.Length);
            } else {
                return "".PadLeft(spaces);
            }
        }

        public static void PrintEmptyLine(int emptyLineCount = 1) {
            for (int i = 0; i < emptyLineCount; i++) {
                Console.WriteLine();
            }
        }
    }
}