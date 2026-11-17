namespace TTOptimize.ConsoleUI.View;

public readonly record struct Conxel(
    string Glyph = " ",
    RgbColor? Foreground = null, // null = terminal default color
    RgbColor? Background = null
);

