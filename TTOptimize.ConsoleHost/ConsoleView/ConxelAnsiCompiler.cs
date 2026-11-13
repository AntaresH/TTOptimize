using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.ConsoleHost.ConsoleView;

public class ConxelAnsiCompiler
{
    public string Compile(ConxelCanvas canvas)
    {
        var sb = new StringBuilder(canvas.Width * canvas.Height * 2);

        for (int y = 0; y < canvas.Height; y++)
        {
            RgbColor? currentFg = null;
            RgbColor? currentBg = null;

            for (int x = 0; x < canvas.Width; x++)
            {
                Conxel cell = canvas.GetAt(x, y);

                if (cell.Foreground != currentFg)
                {
                    if (cell.Foreground is RgbColor fg)
                        sb.Append($"\u001b[38;2;{fg.R};{fg.G};{fg.B}m");
                    else
                        sb.Append("\u001b[39m"); 

                    currentFg = cell.Foreground;
                }

                if (cell.Background != currentBg)
                {
                    if (cell.Background is RgbColor bg)
                        sb.Append($"\u001b[48;2;{bg.R};{bg.G};{bg.B}m");
                    else
                        sb.Append("\u001b[49m"); 

                    currentBg = cell.Background;
                }

                sb.Append(cell.Symbol);
            }

            if (currentFg != null || currentBg != null)
            {
                sb.Append("\u001b[0m");
            }

            sb.AppendLine();
        }

        return sb.ToString();
    }
}
