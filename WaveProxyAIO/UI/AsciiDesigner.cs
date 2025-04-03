/* ASCII Generator: https://www.asciiart.eu/text-to-ascii-art
 * Font:            Small Slant
 * Border:          None
 * Comment Style:   None
*/

namespace WaveProxyAIO.UI {
    internal class AsciiDesigner {
        public static string[] WaveAsciiArt() {
            return [
                    @"  _      _____ _   ______   ___   ________  ",
                    @" | | /| / / _ | | / / __/  / _ | /  _/ __ \ ",
                    @" | |/ |/ / __ | |/ / _/   / __ |_/ // /_/ / ",
                    @" |__/|__/_/ |_|___/___/  /_/ |_/___/\____/  ",
                ];
        }

        public static string[] ScraperAsciiArt() {
            return [
                    @"   ____________  ___   ___  _______ ",
                    @"  / __/ ___/ _ \/ _ | / _ \/ __/ _ \",
                    @" _\ \/ /__/ , _/ __ |/ ___/ _// , _/",
                    @"/___/\___/_/|_/_/ |_/_/  /___/_/|_| "
                ];
        }

        public static string[] CheckerAsciiArt() {
            return [
                    @"   _______ _____________ _________ ",
                    @"  / ___/ // / __/ ___/ //_/ __/ _ \",
                    @" / /__/ _  / _// /__/ ,< / _// , _/ ",
                    @" \___/_//_/___/\___/_/|_/___/_/|_| "
                ];
        }

        public static string[] InfoAsciiArt() {
            return [
                    @"   _____  __________ ",
                    @"  /  _/ |/ / __/ __ \",
                    @" _/ //    / _// /_/ /",
                    @"/___/_/|_/_/  \____/ "
                ];
        }

        public static string[] MainMenuSelectionText() {
            return [
                    @"[1] Scrape Proxies",
                    @"[2] Check Proxies",
                    @"[3] Information",
                    @"[9] Exit"
                ];
        }

        public static string[] InfoText() {
            return [
                    @"This is a learning project",
                    @"",
                    @"Wave Proxy AIO",
                    @"Made with <3 by PhenixHD",
                    @"Github - https://github.com/PhenixHD"
                ];
        }
    }
}