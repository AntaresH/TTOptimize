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
        var conxelCanvas = new ConxelCanvas(_eventLogHeight - 2, _eventLogWidth - 4);

        conxelCanvas.WriteText(0, 0, WidgetBase.CenterText(Strings.EventLogTitle, _eventLogWidth), ColorHelper.LightSetRed);

        int logRow = 2;
        foreach (var logEntry in paneData.History.Reverse().Take(_eventLogHeight-5))
        {
            var fgColor = GetForegroundColor(logEntry.Agent.PlayerId);

            conxelCanvas.WriteText(logRow++, 0, $"#{logEntry.TurnNumber}: {logEntry.Agent.Name} - {logEntry.Action.Description}", fgColor);
        }

        return conxelCanvas.WithSingleBorder(ColorHelper.LightSetRed);
    }

    private RgbColor GetForegroundColor(int activePlayer)
    {
        return activePlayer switch
        {
            1 => ColorHelper.LightSetYellow,
            2 => ColorHelper.Salmon,
            3 => ColorHelper.LightSetGreen,
            4 => ColorHelper.LightSetBlue,
            _ => throw new InvalidOperationException("Player count mzst be between 1 and 4!")
        };
    }
}
