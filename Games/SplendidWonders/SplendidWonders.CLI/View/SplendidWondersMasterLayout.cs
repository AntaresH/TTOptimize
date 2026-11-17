using TTOptimize.ConsoleUI.View;

namespace SplendidWonders.CLI.View;

public class SplendidWondersMasterLayout : IConxelMasterLayout
{
    private const int MarginX = 1;
    private const int MarginY = 1;
    private const int Gap = 2;

    private const int BoardWidth = 140;
    private const int BoardHeight = 42;

    private const int EventsWidth = 80;
    private const int EventsHeight = 40; 

    private const int BottomPaneWidth = BoardWidth + Gap + EventsWidth;
    private const int BottomPaneHeight = 12;

    public IReadOnlyList<ConxelPane> CreatePanes()
    {
        int eventsStartX = MarginX + BoardWidth + Gap;
        int bottomStartY = MarginY + BoardHeight + 5;

        return new List<ConxelPane>
        {
            new ConxelPane(PaneId.Board, MarginY, MarginX, BoardHeight, BoardWidth),
            new ConxelPane(PaneId.Events, MarginY, eventsStartX, EventsHeight, EventsWidth),
            new ConxelPane(PaneId.Commands, bottomStartY, MarginX, BottomPaneHeight, BottomPaneWidth),
            new ConxelPane(PaneId.Results, bottomStartY, MarginX,  BottomPaneHeight, BottomPaneWidth)
        };
    }
}
