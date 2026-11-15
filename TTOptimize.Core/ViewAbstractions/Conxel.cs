using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core.ViewAbstractions;

public readonly record struct RgbColor(byte R, byte G, byte B)
{
    public static readonly RgbColor Orange = new RgbColor(204, 85, 0);
    public static readonly RgbColor Turqoise = new RgbColor(0, 128, 128);
    public static readonly RgbColor DarkMagenta = new RgbColor(160, 0, 80);
    public static readonly RgbColor WarmWhite = new RgbColor(255, 250, 220);
    public static readonly RgbColor FullWhite = new RgbColor(255, 255, 255);
    public static readonly RgbColor DarkRed = new RgbColor(80, 0, 0);
}

public readonly record struct Conxel(
    string Glyph = " ",
    RgbColor? Foreground = null, // null = terminal default color
    RgbColor? Background = null
);

