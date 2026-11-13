using System.Globalization;

namespace TTOptimize.Core.ViewAbstractions;

public class ConxelCanvas
{
    private readonly Conxel[,] _buffer;
    public int Columns { get; }
    public int Rows { get; }

    public ConxelCanvas(int rows, int columns)
    {
        Columns = columns;
        Rows = rows;
        _buffer = new Conxel[rows, columns];
        Clear();
    }

    public void Clear()
    {
        for (int row = 0; row < Rows; row++)
            for (int column = 0; column < Columns; column++)
                _buffer[row, column] = new Conxel(" ");
    }

    public void Blit(Conxel[,] source, int startRow, int startColumns)
    {
        int srcRows = source.GetLength(0);
        int srcColumns = source.GetLength(1);

        for (int row = 0; row < srcRows; row++)
        {
            int targetRow = startRow + row;
            if (targetRow < 0 || targetRow >= Rows) continue;

            for (int column = 0; column < srcColumns; column++)
            {
                int targetColumns = startColumns + column;
                if (targetColumns < 0 || targetColumns >= Columns) continue;

                _buffer[targetRow, targetColumns] = source[row, column];
            }
        }
    }
    public void Blit(ConxelCanvas source, int startRow, int startColumn)
    {
        Blit(source._buffer, startRow, startColumn);
    }

    public void WriteText(int startRow, int startColumn, string text, RgbColor? foreground = null, RgbColor? background = null)
    {
        if (startRow < 0 || startRow >= Rows || string.IsNullOrEmpty(text))
        {
            return;
        }

        var enumerator = StringInfo.GetTextElementEnumerator(text);
        int offset = 0;

        while (enumerator.MoveNext())
        {
            int targetColumn = startColumn + offset;
            if (targetColumn >= Columns) break;

            if (targetColumn >= 0)
            {
                string element = enumerator.GetTextElement();
                RgbColor? effectiveBg = background ?? _buffer[startRow, targetColumn].Background;
                _buffer[startRow, targetColumn] = new Conxel(element, foreground, effectiveBg);
            }

            offset++;
        }
    }

    public Conxel this[int row, int column]
    {
        get => _buffer[row, column];
        set => _buffer[row, column] = value;
    }

    public Conxel GetAt(int row, int column) => _buffer[row, column];
}
