using System.Globalization;

namespace TTOptimize.ConsoleUI;

public static class ConxelWidthHelper
{
    public static int GetVisualWidth(string? text)
    {
        if (string.IsNullOrEmpty(text)) return 0;

        // Pontosan 1-nek számolja a templomot, a kártyát és minden más szövegelemet
        return new StringInfo(text).LengthInTextElements;
    }
}
