namespace TTOptimize.Core.ViewAbstractions;

public class ConxelCanvas
{
    private readonly Conxel[,] _buffer;
    public int Width { get; }
    public int Height { get; }

    public ConxelCanvas(int width, int height)
    {
        Width = width;
        Height = height;
        _buffer = new Conxel[height, width];
        Clear();
    }

    public void Clear()
    {
        for (int y = 0; y < Height; y++)
            for (int x = 0; x < Width; x++)
                _buffer[y, x] = new Conxel(' ');
    }

    public void Blit(ConxelCanvas source, int startX, int startY)
    {
        for (int y = 0; y < source.Height; y++)
        {
            int targetY = startY + y;
            if (targetY < 0 || targetY >= Height) continue;

            for (int x = 0; x < source.Width; x++)
            {
                int targetX = startX + x;
                if (targetX < 0 || targetX >= Width) continue;

                _buffer[targetY, targetX] = source[y, x];
            }
        }
    }
    public Conxel this[int y, int x]
    {
        get => _buffer[y, x];
        set => _buffer[y, x] = value;
    }

    public Conxel GetAt(int x, int y) => _buffer[y, x];
}
