namespace TTOptimize.ConsoleUI.View;

public class ConxelPane
{
    public PaneId PaneId { get; }
    public int StartX { get; }
    public int StartY { get; }
    public ConxelCanvas Canvas { get; }

    public ConxelPane(PaneId paneId, int startY, int startX, int rows, int columns)
    {
        PaneId = paneId;
        StartX = startX;
        StartY = startY;
        Canvas = new ConxelCanvas(rows, columns);
    }
}