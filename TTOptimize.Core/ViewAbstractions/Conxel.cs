using System;
using System.Collections.Generic;
using System.Text;

namespace TTOptimize.Core.ViewAbstractions;

public readonly record struct RgbColor(byte R, byte G, byte B)
{
}

public readonly record struct Conxel(
    string Glyph = " ",
    RgbColor? Foreground = null, // null = terminal default color
    RgbColor? Background = null
);

