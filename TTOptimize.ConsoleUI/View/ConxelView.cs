using Microsoft.Extensions.DependencyInjection;
using TTOptimize.ConsoleUI.Input;
using TTOptimize.Core.Setup;
using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.ConsoleUI.View;

public class ConxelView : IGameView
{
    private readonly Dictionary<PaneId, ConxelPane> _panes;
    private readonly ConxelCompiler _compiler;
    private readonly ICommandTerminal _terminal;
    private readonly IConxelPaneBuilder<BoardRenderData> _boardBuilder;
    private readonly IConxelPaneBuilder<EventLogRenderData> _eventLogBuilder;
    private readonly IConxelPaneBuilder<ResultsRenderData> _resultsBuilder;
    private readonly IConxelPaneBuilder<CommandRenderData> _commandBuilder;

    public ConxelView(
        ICommandTerminal terminal,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IConxelMasterLayout masterLayout,
        ConxelCompiler compiler,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IConxelPaneBuilder<BoardRenderData> boardBuilder,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IConxelPaneBuilder<EventLogRenderData> eventLogBuilder,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IConxelPaneBuilder<ResultsRenderData> resultsBuilder,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IConxelPaneBuilder<CommandRenderData> commandBuilder)
    {
        _panes = masterLayout.CreatePanes().ToDictionary(p => p.PaneId);
        _compiler = compiler;
        _terminal = terminal;
        _boardBuilder = boardBuilder;
        _eventLogBuilder = eventLogBuilder;
        _resultsBuilder = resultsBuilder;
        _commandBuilder = commandBuilder;

        _terminal.OutputChanged += Terminal_OutputChanged;
    }

    public void InitializeScreen()
    {
        Console.Clear();
        Console.CursorVisible = false;
    }

    public void RefreshBoard(BoardRenderData data)
        => UpdatePane(PaneId.Board, _boardBuilder.Build(data));

    public void RefreshEvents(EventLogRenderData data)
        => UpdatePane(PaneId.Events, _eventLogBuilder.Build(data));

    public void RefreshCommands(CommandRenderData data)
        => UpdatePane(PaneId.Commands, _commandBuilder.Build(data));

    public void ShowResults(ResultsRenderData data)     
        => UpdatePane(PaneId.Results, _resultsBuilder.Build(data));

    private void Terminal_OutputChanged(CommandRenderData data)
    {
        UpdatePane(PaneId.Commands, _commandBuilder.Build(data));
    }

    private void UpdatePane(PaneId paneId, ConxelCanvas content)
    {
        if (!_panes.TryGetValue(paneId, out var pane)) return;

        pane.Canvas.Clear();
        pane.Canvas.Blit(content, 0, 0);
        pane.Canvas.CursorPosition = content.CursorPosition;
        _compiler.RenderPane(pane);
    }

    public void Dispose()
    {
        _terminal.OutputChanged -= Terminal_OutputChanged;
    }
}