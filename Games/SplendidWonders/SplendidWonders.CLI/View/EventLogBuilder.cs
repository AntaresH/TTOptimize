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
                conxelCanvas[i, j] = new Conxel(" ", ColorHelper.WarmWhite, ColorHelper.DarkRed);

        conxelCanvas.WriteText(0, 0, WidgetBase.CenterText(Strings.EventLogTitle, _eventLogWidth).Replace(' ', '-'), ColorHelper.LightSetRed);

        int logRow = 2;
        foreach (var logEntry in paneData.History.Reverse().Take(_eventLogHeight-3))
        {
            var fgColor = logRow % 2 == 1 ? ColorHelper.LightSetYellow : ColorHelper.WarmWhite;

            conxelCanvas.WriteText(logRow++, 0, $"#{logEntry.TurnNumber}: {logEntry.Agent.Name} - {logEntry.Action.Description}", fgColor);
        }

        conxelCanvas.WriteText(_eventLogHeight - 1, 0, new string('-', _eventLogWidth), ColorHelper.LightSetRed);

        return conxelCanvas;
    }
}
