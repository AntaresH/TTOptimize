using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace SplendidWonders.CLI.View
{
    public static class WidgetRenderingHelper
    {
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
