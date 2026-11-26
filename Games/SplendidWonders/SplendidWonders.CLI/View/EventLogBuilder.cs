using SplendidWonders.CLI.View.Widgets;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View;

public class EventLogBuilder : IConxelPaneBuilder<EventLogRenderData>
{
    private int _eventLogHeight = LayoutConstants.EVENTSHEIGHT;
    private int _eventLogWidth = LayoutConstants.EVENTSWIDTH;

    public ConxelCanvas Build(EventLogRenderData paneData)
    {
        var conxelCanvas = new ConxelCanvas(_eventLogHeight, _eventLogWidth);

        for (int i = 2; i < _eventLogHeight - 1; i++)
            for (int j = 0; j < _eventLogWidth; j++)
                conxelCanvas[i, j] = new Conxel(" ", ColorHelper.WarmWhite, ColorHelper.LightBlackBackground);

        conxelCanvas.WriteText(0, 0, WidgetBase.CenterText(Strings.EventLogTitle, _eventLogWidth).Replace(' ', '-'), ColorHelper.LightSetRed);

        int logRow = 2;
        foreach (var logEntry in paneData.History.Reverse().Take(_eventLogHeight-3))
        {
            var fgColor = GetPlayerSpecificForeground(logEntry.Player.Seat);

            conxelCanvas.WriteText(logRow++, 0, $"#{logEntry.TurnNumber}: {logEntry.Player.Name} - {logEntry.Action.Description}", fgColor);
        }

        return conxelCanvas;
    }

    private RgbColor GetPlayerSpecificForeground(int playerId)
    {
        return playerId switch
        {
            1 => ColorHelper.PaleSetBlue,
            2 => ColorHelper.PaleSetYellow,
            3 => ColorHelper.PaleSetGreen,
            4 => ColorHelper.PaleSetRed,
            _ => throw new InvalidOperationException("Player count mzst be between 1 and 4!")
        };
    }
}
