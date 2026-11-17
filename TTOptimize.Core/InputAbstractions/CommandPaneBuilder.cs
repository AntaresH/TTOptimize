using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.Engine;

public class CommandPaneBuilder : IPaneBuilder<CommandRenderData>
{
    private const int Width = 149;
    private const int Height = 12;

    public ConxelCanvas Build(CommandRenderData data)
    {
        var canvas = new ConxelCanvas(Width, Height);

        int maxVisibleLines = Height - 2;
        var visibleLines = data.RenderLines.TakeLast(maxVisibleLines).ToList();

        for (int i = 0; i < visibleLines.Count; i++)
        {
            canvas.WriteText(2, 1 + i, visibleLines[i], new RgbColor(230, 230, 230));
        }

        return canvas;
    }
}
