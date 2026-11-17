using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View;

public class SplendidWondersEventLogBuilder : IConxelPaneBuilder<EventLogRenderData>
{
    private int _eventLogColumns = 80;
    private int _eventLogRows = 40;

    public ConxelCanvas Build(EventLogRenderData paneData)
    {
        var conxelCanvas = new ConxelCanvas(_eventLogRows, _eventLogColumns);

        for (int i = 1; i < _eventLogRows; i++)
            for (int j = 0; j < _eventLogColumns; j++)
                conxelCanvas[i, j] = new Conxel(" ", ColorHelper.WarmWhite, ColorHelper.DarkRed);

        conxelCanvas.WriteText(0, 0, WidgetBase.CenterText(Strings.EventLogTitle, _eventLogColumns).Replace(' ', '-'), ColorHelper.LightSetRed);

        int logRow = 1;
        foreach (var logEntry in paneData.History.Reverse())
        {
            var fgColor = logRow % 2 == 1 ? ColorHelper.LightSetYellow : ColorHelper.WarmWhite;

            conxelCanvas.WriteText(logRow++, 0, $"#{logEntry.TurnNumber}: {logEntry.Agent.Name} - {logEntry.Action.Description}", fgColor);
        }

        return conxelCanvas;
    }
}
