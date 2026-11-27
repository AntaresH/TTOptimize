using System.Diagnostics;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.Engine;

public class GameSession
{
    private readonly GameOrchestrator _orchestrator;
    private readonly IGameRules _rules;
    private readonly IGameView? _view;
    private readonly IGameInput _input;
    private readonly List<ActionLogEntry> _actionHistory;

    public GameSession(GameOrchestrator orchestrator, IGameRules rules, IGameView? view, IGameInput input)
    {
        _orchestrator = orchestrator;
        _rules = rules;
        _view = view;
        _input = input;

        _actionHistory = new List<ActionLogEntry>();
    }

    public SessionResult Execute()
    {
        _orchestrator.TurnStarted += OnTurnStarted;
        _orchestrator.ValidationFailed += OnValidationFailed;
        _orchestrator.ActionExecuted += OnActionExecuted;

        IGameState finalState;
        TimeSpan elapsed;
        DateTime startedAt, finishedAt;

        try
        {
            _view?.InitializeScreen();

            long startTime = Stopwatch.GetTimestamp();
            startedAt = DateTime.UtcNow;
            
            finalState = _orchestrator.Run();
            
            elapsed = Stopwatch.GetElapsedTime(startTime);
            finishedAt = DateTime.UtcNow;
        }
        finally
        {
            _orchestrator.TurnStarted -= OnTurnStarted;
            _orchestrator.ValidationFailed -= OnValidationFailed;
            _orchestrator.ActionExecuted -= OnActionExecuted;
        }

        IReadOnlyList<IPlayerResult> finalStandings = _rules.GetFinalStandings(finalState);

        if (_view != null)
        {
            _view.RefreshBoard(new BoardRenderData(finalState, null));
            _view.ShowResults(new ResultsRenderData(finalState, finalStandings));
        }

        var outcome = new MatchOutcome(finalState, finalStandings, _actionHistory);
        var executionInfo = new MatchExecutionInfo(startedAt, finishedAt, elapsed);

        return new SessionResult(outcome, executionInfo);
    }

    private void OnTurnStarted(IGameState currentState, IReadOnlyList<GameAction> validActions, IAgent activeAgent)
    {
        if (activeAgent is HumanAgent)
        {
            _view.RefreshBoard(new BoardRenderData(currentState, validActions));
            _view.RefreshEvents(new EventLogRenderData(_actionHistory));
        }
    }

    private void OnValidationFailed(string invalidCommandText, IAgent activeAgent)
    {
        _input.ValidationFailed(invalidCommandText);
    }

    private void OnActionExecuted(GameAction action, IPlayer player)
    {
        _actionHistory.Add(new ActionLogEntry
        {
            TurnNumber = _actionHistory.Count + 1,
            Action = action,
            Player = player
        });
    }
}
