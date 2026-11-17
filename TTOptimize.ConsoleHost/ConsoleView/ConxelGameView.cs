using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay;
using System.Data.Common;
using TTOptimize.ConsoleHost;
using TTOptimize.ConsoleHost.ConsoleView;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;

public class ConxelGameView : IGameView
{
    private readonly Dictionary<PaneId, ScreenPane> _panes;
    private readonly ConxelAnsiCompiler _compiler;
    private readonly IPaneBuilder<BoardRenderData> _boardBuilder;
    private readonly IPaneBuilder<EventLogRenderData> _eventLogBuilder;

    public ConxelGameView(
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IMasterLayout masterLayout,
        ConxelAnsiCompiler compiler,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IPaneBuilder<BoardRenderData> boardBuilder,
        [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IPaneBuilder<EventLogRenderData> eventLogBuilder)
    {
        _panes = masterLayout.CreatePanes().ToDictionary(p => p.PaneId);
        _compiler = compiler;
        _boardBuilder = boardBuilder;
        _eventLogBuilder = eventLogBuilder;
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
        => throw new NotImplementedException(); // UpdatePane(PaneId.Commands, _commandBuilder.Build(data));

    public void ShowResults(ResultsRenderData data)     // UpdatePane(PaneId.Results, _resultsBuilder.Build(data));
    {
        var swState = (GameState) data.State;
        var swRules = (Rules) data.Rules;

        var players = (List<Player>)swRules.GetFinalStandings(swState);

        Console.WriteLine($"{Strings.GameOverTitle} - {Strings.Winner}: {players.First().Name}");
        Console.WriteLine();
        Console.WriteLine(Strings.FinalScores);

        for (int i = 0; i < players.Count; i++)
        {
            Console.WriteLine($"#{i + 1}: {players[i].Name} - {players[i].Tableau.Sum(c => c.VictoryPoints) + players[i].AcquiredLeaders.Count * 3} {Strings.VP}");
        }
        Console.WriteLine();
    }

    private void UpdatePane(PaneId paneId, ConxelCanvas content)
    {
        if (!_panes.TryGetValue(paneId, out var pane)) return;

        pane.Canvas.Clear();
        pane.Canvas.Blit(content, 0, 0);
        _compiler.RenderPane(pane);
    }


    public void DisplayValidationFailure(string lastValidationFailure)
    {
        Console.WriteLine();
        PrintColoredLine(lastValidationFailure, ConsoleColor.Red);
    }

    private void PrintColoredLine(string text, ConsoleColor color)
    {
        var previousColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ForegroundColor = previousColor;
        Console.WriteLine();
    }
}