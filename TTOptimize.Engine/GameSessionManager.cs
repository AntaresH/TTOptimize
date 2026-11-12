using Microsoft.Extensions.DependencyInjection;
using System.Data;
using System.Diagnostics;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Engine;

public class GameSessionManager
{
    private readonly IServiceProvider _rootServiceProvider;
    private IGameView _currentView;

    private string _lastValidationFailure;

    public GameSessionManager(IServiceProvider rootServiceProvider)
    {
        _rootServiceProvider = rootServiceProvider;
    }

    public void StartNewMatch(string gameId, IGameConfiguration configuration, IReadOnlyList<IPlayerAgent> agents)
    {
        using (IServiceScope scope = _rootServiceProvider.CreateScope())
        {
            IServiceProvider scopeProvider = scope.ServiceProvider;

            var stateFactory = scopeProvider.GetRequiredKeyedService<IGameStateFactory>(gameId);
            var rules = scopeProvider.GetRequiredKeyedService<IGameRules>(gameId);
            var input = scopeProvider.GetRequiredKeyedService<IGameInput>(gameId);

            foreach (var agent in agents)
            {
                if (agent is HumanAgent hAgent)
                {
                    hAgent.SetInputMethod(input);
                }
            }

            _currentView = scopeProvider.GetRequiredKeyedService<IGameView>(gameId);

            var initialState = stateFactory.Create(configuration);

            var orchestrator = new GameOrchestrator(initialState, agents, rules);

            var finalState = RunGame(orchestrator, out TimeSpan elapsed);

            var finalStandings = rules.GetFinalStandings(finalState);
            _currentView.RenderFinalState(finalState, finalStandings);

            Console.WriteLine($"Futási idő: {Math.Round(elapsed.TotalSeconds, 4)} másodperc");
        }
    }

    private IGameState RunGame(GameOrchestrator orchestrator, out TimeSpan elapsed)
    {
        if (_currentView != null)
        {
            orchestrator.TurnStarted += Orchestrator_TurnStarted;
            orchestrator.ValidationFailed += Orchestrator_ValidationFailed;
        }
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
        }
    }

    private void Orchestrator_TurnStarted(IGameState currentState, IReadOnlyList<GameAction> validActions, IPlayerAgent activeAgent)
    {
        if (activeAgent is HumanAgent)
        {
            _currentView?.Render(currentState, validActions, _lastValidationFailure);
            _lastValidationFailure = null;
        }
    }

    private void Orchestrator_ValidationFailed(string validationFailureMessage, IPlayerAgent activeAgent)
    {
        _lastValidationFailure = validationFailureMessage;
    }
}