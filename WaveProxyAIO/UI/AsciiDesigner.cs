/* ASCII Generator: https://www.asciiart.eu/text-to-ascii-art
 * Font:            Small Slant
 * Border:          None
 * Comment Style:   None
*/

namespace WaveProxyAIO.UI {
    internal class AsciiDesigner {
        public static string[] Wave() {
            return new string[] {
                @"  _      _____ _   ______   ___   ________  ",
                @" | | /| / / _ | | / / __/  / _ | /  _/ __ \ ",
                @" | |/ |/ / __ | |/ / _/   / __ |_/ // /_/ / ",
                @" |__/|__/_/ |_|___/___/  /_/ |_/___/\____/  ",
            };
        }

        public static string[] Scraper() {
            return new string[] {
                @"   ____________  ___   ___  _______ ",
                @"  / __/ ___/ _ \/ _ | / _ \/ __/ _ \",
                @" _\ \/ /__/ , _/ __ |/ ___/ _// , _/",
                @"/___/\___/_/|_/_/ |_/_/  /___/_/|_| "
            };
        }

        public static string[] Checker() {
            return new string[] {
                @"   _______ _____________ _________ ",
                @"  / ___/ // / __/ ___/ //_/ __/ _ \",
                @" / /__/ _  / _// /__/ ,< / _// , _/",
                @" \___/_//_/___/\___/_/|_/___/_/|_| "
            };
        }

        public static string[] Info() {
            return new string[] {
                @"   _____  __________ ",
                @"  /  _/ |/ / __/ __ \",
                @" _/ //    / _// /_/ /",
                @"/___/_/|_/_/  \____/ "
            };
        }

        // Multi-Line Main Menu Selection
        public static string[] GetMainMenuSelection() {
            return new string[] {
        @"[1] Scrape Proxies",
        @"[2] Check Proxies",
        @"[3] Host Rotating",
        @"[4] Settings",
        @"[9] Information",
        @"[0] Exit"
    };
        }

        public static string[] GetSettingsMenuSelection() {
            return new string[] {
        @"[1] Timeout",
        @"[2] Remove Duplicates"
    };
        }

        public static string[] GetInformationMenuSelection() {
            return new string[] {
        @"[Application]",
        @"Developer Build - Unfinished",
        @"",
        @"[Developer]",
        @"Github: https://github.com/PhenixHD"
    };
        }

    }
}