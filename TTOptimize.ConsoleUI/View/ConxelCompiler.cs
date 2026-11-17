using System.Text;

namespace TTOptimize.ConsoleUI.View;

public class ConxelCompiler
{
    public void RenderPane(ConxelPane pane)
    {
        var canvas = pane.Canvas;
        var sb = new StringBuilder(canvas.Width * canvas.Height * 2);

        for (int row = 0; row < canvas.Height; row++)
        {
            sb.Append($"\u001b[{pane.StartY + row + 1};{pane.StartX + 1}H");

            RgbColor? currentFg = null;
            RgbColor? currentBg = null;

            for (int column = 0; column < canvas.Width; column++)
            {
                Conxel cell = canvas.GetAt(row, column);

                if (cell.Foreground != currentFg)
                {
                    sb.Append(cell.Foreground is RgbColor fg
                        ? $"\u001b[38;2;{fg.R};{fg.G};{fg.B}m"
                        : "\u001b[39m");
                    currentFg = cell.Foreground;
                }

                if (cell.Background != currentBg)
                {
                    sb.Append(cell.Background is RgbColor bg
                        ? $"\u001b[48;2;{bg.R};{bg.G};{bg.B}m"
                        : "\u001b[49m");
                    currentBg = cell.Background;
                }

                sb.Append(cell.Glyph);
            }

            if (currentFg != null || currentBg != null)
            {
                sb.Append("\u001b[0m");
            }
        }

        Console.Write(sb.ToString());
    }
}
