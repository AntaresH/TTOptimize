using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core.ViewAbstractions;

public readonly record struct RgbColor(byte R, byte G, byte B)
{
    public static readonly RgbColor FullWhite = new RgbColor(255, 255, 255);
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

    public static readonly RgbColor Orange = new RgbColor(204, 85, 0);
    public static readonly RgbColor Turqoise = new RgbColor(0, 128, 128);
    public static readonly RgbColor DarkMagenta = new RgbColor(160, 0, 80);
}

public readonly record struct Conxel(
    string Glyph = " ",
    RgbColor? Foreground = null, // null = terminal default color
    RgbColor? Background = null
);

