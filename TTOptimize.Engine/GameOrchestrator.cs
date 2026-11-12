using Microsoft.VisualBasic;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Engine;

public class GameOrchestrator
{
    private readonly IGameState _initialState;
    private readonly IReadOnlyList<IPlayerAgent> _playerAgents;
    private readonly IGameRules _rules;
    public event Action<IGameState, IReadOnlyList<GameAction>, IPlayerAgent>? TurnStarted;
    public event Action<string, IPlayerAgent>? ValidationFailed;

    public GameOrchestrator(
        IGameState initialState,
        IReadOnlyList<IPlayerAgent> playerAgents,
        IGameRules rules)
    {
        _initialState = initialState;
        _playerAgents = playerAgents;
        _rules = rules;
    }

    public IGameState Run()
    {
        IGameState currentState = _initialState;

        while (currentState.GameProgression != GameProgression.GameOver)
        {
            var validActions = _rules.Explore(currentState);
            var activeAgent = _playerAgents[currentState.ActivePlayerId - 1];

            TurnStarted?.Invoke(currentState, validActions, activeAgent);

            var command = activeAgent.DecideCommand(currentState, validActions);

            if (_rules.Validate(command.Action))
            {
                currentState = _rules.Reduce(currentState, command.Action);
            }
            else
            {
                ValidationFailed?.Invoke(command.CommandText, activeAgent);
            }
        }

        return currentState;
    }
}