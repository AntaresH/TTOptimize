using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.ConsoleHost.ConsoleView;

public class ConxelAnsiCompiler
{
    public string Compile(ConxelCanvas canvas)
    {
        var sb = new StringBuilder(canvas.Columns * canvas.Rows * 2);

        for (int row = 0; row < canvas.Rows; row++)
        {
            RgbColor? currentFg = null;
            RgbColor? currentBg = null;

            for (int column = 0; column < canvas.Columns; column++)
            {
                Conxel cell = canvas.GetAt(row, column);

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

                sb.Append(cell.Glyph);
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
