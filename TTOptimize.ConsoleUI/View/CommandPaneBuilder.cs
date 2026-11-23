using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.ConsoleUI.View;

public class CommandPaneBuilder : IConxelPaneBuilder<CommandRenderData>
{
    private int _columns;
    private int _rows;

    public CommandPaneBuilder(int rows, int columns)
    {
        _rows = rows;
        _columns = columns;
    }

    public ConxelCanvas Build(CommandRenderData data)
    {
        var canvas = new ConxelCanvas(_rows, _columns);

        int innerWidth = _columns - 2;
        int maxVisibleLines = _rows - 2;
        var visibleLines = data.Lines.TakeLast(maxVisibleLines).ToList();

        for (int i = 0; i < visibleLines.Count; i++)
        {
            int y = 1 + i;
            var line = visibleLines[i];

            if (line.LineBackground is RgbColor lineBg)
            {
                canvas.WriteText(y, 1, new string(' ', innerWidth), null, lineBg);
            }

            if (line.IsDivider)
            {
                canvas.WriteText(y, 1, new string('-', innerWidth), line.DividerForeground, line.LineBackground);
                continue;
            }

            int currentX = 1;
            foreach (var span in line.LeftSpans)
            {
                canvas.WriteText(y, currentX, span.Text, span.Foreground, span.Background ?? line.LineBackground);
                currentX += ConxelWidthHelper.GetVisualWidth(span.Text);
            }

            if (line.RightSpans.Count > 0)
            {
                int totalRightWidth = line.RightSpans.Sum(s => ConxelWidthHelper.GetVisualWidth(s.Text));
                int rightStartX = Math.Max(currentX + 1, 1 + innerWidth - totalRightWidth);

                foreach (var span in line.RightSpans)
                {
                    canvas.WriteText(y, rightStartX, span.Text, span.Foreground, span.Background ?? line.LineBackground);
                    rightStartX += ConxelWidthHelper.GetVisualWidth(span.Text);
                }
            }
        }

        canvas.CursorPosition = (data.Lines.Count + 1, 0);

        return canvas;
    }
}
