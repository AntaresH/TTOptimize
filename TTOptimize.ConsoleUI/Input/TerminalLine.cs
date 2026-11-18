using TTOptimize.ConsoleUI.View;

namespace TTOptimize.ConsoleUI.Input;

public readonly record struct TerminalSpan(
    string Text,
    RgbColor? Foreground = null,
    RgbColor? Background = null);

public class TerminalLine
{
    public List<TerminalSpan> LeftSpans { get; } = new();
    public List<TerminalSpan> RightSpans { get; } = new();
    public bool IsDivider { get; init; }
    public RgbColor? DividerForeground { get; init; }
    public RgbColor? LineBackground { get; set; }
}