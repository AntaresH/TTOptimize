using System.Globalization;

namespace TTOptimize.ConsoleUI.View;

public class ConxelCanvas
{
    private readonly Conxel[,] _buffer;
    public int Width => Columns;
    public int Height => Rows;
    public int Columns { get; }
    public int Rows { get; }
    private static string _defaultHorizSingleBorderSymbol = "-";
    private static string _defaultVertSingleBorderSymbol = "|";
    private static string _defaultHorizDoubleBorderSymbol = "=";
    private static string _defaultVertDoubleBorderSymbol = "\u2551";
    private static string _defaultCornerSymbol = "+";

    public (int Y, int X)? CursorPosition { get; set; }

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

    public ConxelCanvas WithCorners(RgbColor borderColor)
    {
        _buffer[0, 0] = new Conxel(_defaultCornerSymbol, borderColor);
        _buffer[0, Columns - 1] = new Conxel(_defaultCornerSymbol, borderColor);
        _buffer[Rows - 1, 0] = new Conxel(_defaultCornerSymbol, borderColor);
        _buffer[Rows - 1, Columns - 1] = new Conxel(_defaultCornerSymbol, borderColor);

        return this;
    }

    public ConxelCanvas WithSingleBorder(RgbColor borderColor)
    {
        return WithBorder(borderColor, _defaultVertSingleBorderSymbol, _defaultHorizSingleBorderSymbol);
    }

    public ConxelCanvas WithDoubleBorder(RgbColor borderColor)
    {
        return WithBorder(borderColor, _defaultVertDoubleBorderSymbol, _defaultHorizDoubleBorderSymbol);
    }

    protected ConxelCanvas WithBorder(RgbColor borderColor, string vertBorder, string horizBorder)
    {
        var borderedCanvas = new ConxelCanvas(Rows + 2, Columns + 2);
        borderedCanvas.Blit(_buffer, 1, 1);

        borderedCanvas.WriteText(0, 1, string.Concat(Enumerable.Repeat(horizBorder, Columns)), borderColor);

        for (int i = 1; i < borderedCanvas.Rows - 1; i++)
        {
            borderedCanvas[i, 0] = new Conxel(vertBorder, borderColor);
            borderedCanvas[i, borderedCanvas.Columns - 1] = new Conxel(vertBorder, borderColor);
        }

        borderedCanvas.WriteText(borderedCanvas.Rows - 1, 1, string.Concat(Enumerable.Repeat(horizBorder, Columns)), borderColor);

        return borderedCanvas;
    }

    public Conxel this[int row, int column]
    {
        get => _buffer[row, column];
        set => _buffer[row, column] = value;
    }

    public Conxel GetAt(int row, int column) => _buffer[row, column];

    public void WriteText(int v1, int v2, object formattedResultString)
    {
        throw new NotImplementedException();
    }
}
