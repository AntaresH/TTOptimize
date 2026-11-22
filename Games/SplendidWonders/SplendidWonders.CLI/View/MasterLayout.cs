using TTOptimize.ConsoleUI.View;

namespace SplendidWonders.CLI.View;

public class MasterLayout : IConxelMasterLayout
{
    private const int MarginX = LayoutConstants.MARGINX;
    private const int MarginY = LayoutConstants.MARGINY;
    private const int PaneGap = LayoutConstants.PANEGAP;

    private const int BoardWidth = LayoutConstants.BOARDWIDTH;
    private const int BoardHeight = LayoutConstants.BOARDHEIGHT;
    private const int EventsWidth = LayoutConstants.EVENTSWIDTH;
    private const int EventsHeight = LayoutConstants.EVENTSHEIGHT;

    private const int BottomPaneWidth = BoardWidth + PaneGap + EventsWidth;
    private const int BottomPaneHeight = LayoutConstants.BOTTOMPANEHEIGHT;

    public IReadOnlyList<ConxelPane> CreatePanes()
    {
        int eventsStartX = MarginX + BoardWidth + PaneGap;
        int bottomStartY = MarginY + BoardHeight;

        return new List<ConxelPane>
        {
            new ConxelPane(PaneId.Board, MarginY, MarginX, BoardHeight, BoardWidth),
            new ConxelPane(PaneId.Events, MarginY, eventsStartX, EventsHeight, EventsWidth),
            new ConxelPane(PaneId.Commands, bottomStartY, MarginX, BottomPaneHeight, BottomPaneWidth),
            new ConxelPane(PaneId.Results, bottomStartY, MarginX,  BottomPaneHeight, BottomPaneWidth)
        };
    }
}
