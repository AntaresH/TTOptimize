namespace TTOptimize.Core.ViewAbstractions;


public class ScreenPane
{
    public PaneId PaneId { get; }
    public int StartX { get; }
    public int StartY { get; }
    public ConxelCanvas Canvas { get; }

    public ScreenPane(PaneId paneId, int startY, int startX, int rows, int columns)
    {
        PaneId = paneId;
        StartX = startX;
        StartY = startY;
        Canvas = new ConxelCanvas(rows, columns);
    }
}