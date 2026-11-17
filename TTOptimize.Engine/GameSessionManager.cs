using Microsoft.Extensions.DependencyInjection;
using System.Data;
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
    private IGameRenderer _renderer;
    private IGameView _view;

    private string _lastValidationFailure;
    private readonly List<ActionLogEntry> _actionHistory = new();

    public GameSessionManager(IServiceProvider rootServiceProvider)
    {
        _rootServiceProvider = rootServiceProvider;
    }

    public void StartNewMatch(string gameId, IGameConfiguration configuration, IReadOnlyList<IPlayerAgent> agents)
    {
        using (IServiceScope scope = _rootServiceProvider.CreateScope())
        {
            IServiceProvider scopeProvider = scope.ServiceProvider;

            var settings = scopeProvider.GetRequiredService<ISettingsService>();
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

            _renderer = scopeProvider.GetRequiredKeyedService<IGameRenderer>(gameId);
            _view = scopeProvider.GetRequiredKeyedService<IGameView>(gameId);

            var initialState = stateFactory.Create(configuration);

            var orchestrator = new GameOrchestrator(initialState, agents, rules);

            var finalState = RunGame(orchestrator, out TimeSpan elapsed);

            if (agents.OfType<HumanAgent>().Any())
                _renderer.Render(finalState, null);

            _view.DisplayGameOver(finalState, rules);

            string interactiveModeArg = settings.GetSetting(TTOptArguments.INTERACTIVE_MODE);
            bool interActiveMode = bool.TryParse(interactiveModeArg, out bool parsed) ? parsed : true;

            if (!interActiveMode)
            {
                foreach (var logEntry in _actionHistory)
                {
                    Console.WriteLine($"#{logEntry.TurnNumber}: {logEntry.Agent.Name} - {logEntry.Action.Description}");
                }
            }

            Console.WriteLine($"Futási idő: {Math.Round(elapsed.TotalSeconds, 4)} másodperc");
        }
    }

    private IGameState RunGame(GameOrchestrator orchestrator, out TimeSpan elapsed)
    {
        if (_renderer != null)
        {
            orchestrator.TurnStarted += Orchestrator_TurnStarted;
            orchestrator.ValidationFailed += Orchestrator_ValidationFailed;
            orchestrator.ActionExecuted += Orchestrator_ActionExecuted;
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
            _renderer?.Render(currentState, validActions);

            if (!string.IsNullOrEmpty(_lastValidationFailure))
                _view.DisplayValidationFailure(_lastValidationFailure);

            _lastValidationFailure = null;
        }
    }

    private void Orchestrator_ValidationFailed(string invalidCommandText, IPlayerAgent activeAgent)
    {
        _lastValidationFailure = string.Format(Strings.InvalidCommandType, invalidCommandText);
    }
}