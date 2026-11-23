using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.Engine;

public class GameSessionManager
{
    private readonly IServiceProvider _rootServiceProvider;
    private IGameView _view;
    private IGameInput _input;

    private readonly List<ActionLogEntry> _actionHistory = new();

    public GameSessionManager(IServiceProvider rootServiceProvider)
    {
        _rootServiceProvider = rootServiceProvider;
    }

    public void StartNewMatch(string gameId, IGameConfiguration configuration, IReadOnlyList<IPlayerAgent> agents)
    {
        Console.Clear();

        using (IServiceScope scope = _rootServiceProvider.CreateScope())
        {
            IServiceProvider scopeProvider = scope.ServiceProvider;

            var settings = scopeProvider.GetRequiredService<ISettingsService>();
            var stateFactory = scopeProvider.GetRequiredKeyedService<IGameStateFactory>(gameId);
            var rules = scopeProvider.GetRequiredKeyedService<IGameRules>(gameId);
            _input = scopeProvider.GetRequiredKeyedService<IGameInput>(gameId);

            foreach (var agent in agents)
            {
                if (agent is HumanAgent hAgent)
                {
                    hAgent.SetInputMethod(_input);
                }
            }

            _view = scopeProvider.GetRequiredKeyedService<IGameView>(gameId);

            var initialState = stateFactory.Create(configuration);

            var orchestrator = new GameOrchestrator(initialState, agents, rules);

            var finalState = RunGame(orchestrator, out TimeSpan elapsed);
            var finalStandings = rules.GetFinalStandings(finalState, agents);

            _view.RefreshBoard(new BoardRenderData(finalState, null));
            _view.ShowResults(new ResultsRenderData(finalState, finalStandings));

            Console.WriteLine();
            Console.WriteLine($"Futási idő: {Math.Round(elapsed.TotalSeconds, 4)} másodperc");
        }
    }

    private IGameState RunGame(GameOrchestrator orchestrator, out TimeSpan elapsed)
    {
        orchestrator.TurnStarted += Orchestrator_TurnStarted;
        orchestrator.ValidationFailed += Orchestrator_ValidationFailed;
        orchestrator.ActionExecuted += Orchestrator_ActionExecuted;

        try
        {
            long startTime = Stopwatch.GetTimestamp();
            var finalState = orchestrator.Run();
            elapsed = Stopwatch.GetElapsedTime(startTime);
            return finalState;
        }
        finally
        {
            orchestrator.TurnStarted -= Orchestrator_TurnStarted;
            orchestrator.ValidationFailed -= Orchestrator_ValidationFailed;
            orchestrator.ActionExecuted -= Orchestrator_ActionExecuted;
        }
    }

    private void Orchestrator_ActionExecuted(GameAction action, IPlayerAgent agent)
    {
        _actionHistory.Add(new ActionLogEntry()
        {
            TurnNumber = _actionHistory.Count + 1,
            Action = action,
            Agent = agent
        });
    }

    private void Orchestrator_TurnStarted(IGameState currentState, IReadOnlyList<GameAction> validActions, IPlayerAgent activeAgent)
    {
        if (activeAgent is HumanAgent)
        {
            _view.RefreshBoard(new BoardRenderData(currentState, validActions));
            _view.RefreshEvents(new EventLogRenderData(_actionHistory));
        }
    }

    private void Orchestrator_ValidationFailed(string invalidCommandText, IPlayerAgent activeAgent)
    {
        _input.ValidationFailed(invalidCommandText);
    }
}