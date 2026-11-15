using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public static class WidgetHelper
    {
        public static readonly RgbColor LightSetYellow = new RgbColor(208, 208, 0);
        public static readonly RgbColor LightSetBlue = new RgbColor(80, 145, 255);
        public static readonly RgbColor LightSetGreen = new RgbColor(0, 220, 50);
        public static readonly RgbColor LightSetBrown = new RgbColor(145, 92, 42);
        public static readonly RgbColor LightSetRed = new RgbColor(220, 80, 80);
        public static readonly RgbColor LightSetDefault = new RgbColor(192, 192, 192);
        public static readonly RgbColor DarkSetYellow = new RgbColor(138, 116, 0);
        public static readonly RgbColor DarkSetBlue = new RgbColor(30, 60, 165);
        public static readonly RgbColor DarkSetGreen = new RgbColor(0, 125, 30);
        public static readonly RgbColor DarkSetBrown = new RgbColor(100, 45, 0);
        public static readonly RgbColor DarkSetRed = new RgbColor(180, 10, 20);
        public static readonly RgbColor DarkSetDefault = new RgbColor(96, 96, 96);

        public static string CenterText(string text, int totalWidth)
        {
            if (string.IsNullOrEmpty(text))
            {
                return new string(' ', totalWidth);
            }

            // A valódi, látható szövegelemek (cellák) megszámolása a char-ok helyett!
            var stringInfo = new StringInfo(text);
            int visibleLength = stringInfo.LengthInTextElements;

            if (visibleLength >= totalWidth)
            {
                return stringInfo.SubstringByTextElements(0, totalWidth);
            }

            int leftPadding = (totalWidth - visibleLength) / 2;
            int rightPadding = totalWidth - visibleLength - leftPadding;

            return new string(' ', leftPadding) + text + new string(' ', rightPadding);
        }

        public static (string Line1, string Line2) WrapAndCenterText(string text, int maxWidth)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                string empty = new string(' ', maxWidth);
                return (empty, empty);
            }

            text = text.Trim();

            // 1. Eset: Elfér egyetlen sorban
            if (text.Length <= maxWidth)
            {
                return (CenterText(text, maxWidth), new string(' ', maxWidth));
            }

            // 2. Eset: Szóköz mentén tördeljük 2 sorba
            string[] words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var line1Words = new List<string>();
            var line2Words = new List<string>();
            int currentLength = 0;

            foreach (var word in words)
            {
                // Megnézzük, befér-e még az első sorba (a szóközöket is számolva)
                int spaceNeeded = line1Words.Count > 0 ? 1 : 0;
                if (currentLength + spaceNeeded + word.Length <= maxWidth)
                {
                    line1Words.Add(word);
                    currentLength += spaceNeeded + word.Length;
                }
                else
                {
                    line2Words.Add(word);
                }
            }

            // Ha a legelső szó eleve hosszabb volt, mint a maxWidth, kényszerített vágás kell
            if (line1Words.Count == 0)
            {
                string part1 = text[..maxWidth];
                string part2 = text[maxWidth..];
                return (CenterText(part1, maxWidth), CenterText(TruncateIfNeeded(part2, maxWidth), maxWidth));
            }

            string rawLine1 = string.Join(" ", line1Words);
            string rawLine2 = TruncateIfNeeded(string.Join(" ", line2Words), maxWidth);

            return (CenterText(rawLine1, maxWidth), CenterText(rawLine2, maxWidth));
        }

        private static string TruncateIfNeeded(string text, int maxWidth)
        {
            if (text.Length <= maxWidth) return text;
            // Ha a 2. sorba sem fér bele minden, levágjuk és pontokkal jelezzük
            return maxWidth > 3 ? text[..(maxWidth - 2)] + ".." : text[..maxWidth];
        }
    }
}
