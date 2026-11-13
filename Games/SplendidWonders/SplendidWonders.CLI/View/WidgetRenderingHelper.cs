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
    }
}
