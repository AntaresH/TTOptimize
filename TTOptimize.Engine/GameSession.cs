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
    private readonly IReadOnlyList<IPlayerAgent> _agents;
    private readonly IGameView _view;
    private readonly IGameInput _input;
    private readonly List<ActionLogEntry> _actionHistory;

    public GameSession(
        GameOrchestrator orchestrator,
        IGameRules rules,
        IReadOnlyList<IPlayerAgent> agents,
        IGameView view,
        IGameInput input)
    {
        _orchestrator = orchestrator;
        _rules = rules;
        _agents = agents;
        _view = view;
        _input = input;
        _actionHistory = new List<ActionLogEntry>();
    }

    public MatchResult Execute()
    {
        _orchestrator.TurnStarted += OnTurnStarted;
        _orchestrator.ValidationFailed += OnValidationFailed;
        _orchestrator.ActionExecuted += OnActionExecuted;

        IGameState finalState;
        TimeSpan elapsed;

        try
        {
            _view.InitializeScreen();

            long startTime = Stopwatch.GetTimestamp();
            finalState = _orchestrator.Run();
            elapsed = Stopwatch.GetElapsedTime(startTime);
        }
        finally
        {
            _orchestrator.TurnStarted -= OnTurnStarted;
            _orchestrator.ValidationFailed -= OnValidationFailed;
            _orchestrator.ActionExecuted -= OnActionExecuted;
        }

        IReadOnlyList<IPlayerResult> finalStandings = _rules.GetFinalStandings(finalState, _agents);

        _view.RefreshBoard(new BoardRenderData(finalState, null));
        _view.ShowResults(new ResultsRenderData(finalState, finalStandings));

        return new MatchResult(finalState, finalStandings, _actionHistory, elapsed);
    }

    private void OnTurnStarted(IGameState currentState, IReadOnlyList<GameAction> validActions, IPlayerAgent activeAgent)
    {
        if (activeAgent is HumanAgent)
        {
            _view.RefreshBoard(new BoardRenderData(currentState, validActions));
            _view.RefreshEvents(new EventLogRenderData(_actionHistory));
        }
    }

    private void OnValidationFailed(string invalidCommandText, IPlayerAgent activeAgent)
    {
        _input.ValidationFailed(invalidCommandText);
    }

    private void OnActionExecuted(GameAction action, IPlayerAgent agent)
    {
        _actionHistory.Add(new ActionLogEntry
        {
            TurnNumber = _actionHistory.Count + 1,
            Action = action,
            Agent = agent
        });
    }
}
