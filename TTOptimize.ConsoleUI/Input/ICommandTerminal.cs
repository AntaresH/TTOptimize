using TTOptimize.ConsoleUI.View;

namespace TTOptimize.ConsoleUI.Input;

public interface ICommandTerminal
{
    event Action<CommandRenderData>? OutputChanged;
    void BeginZone();
    void ResetZone();
    void EndZone();

    void Write(string text, RgbColor? foreground = null, RgbColor? background = null);
    void WriteLine(string text = "", RgbColor? foreground = null, RgbColor? background = null);
    void WriteRight(string text, RgbColor? foreground = null, RgbColor? background = null);
    void WriteDivider(RgbColor? foreground = null, RgbColor? background = null);

    void SetActiveLineBackground(RgbColor? background = null);

    void Flush();
}
