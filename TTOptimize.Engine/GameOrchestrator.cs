using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Engine;

public class GameOrchestrator
{
    private readonly IGameState _initialState;
    private readonly IReadOnlyDictionary<int, IAgent> _agentsBySeat;
    private readonly IGameRules _rules;
    public event Action<IGameState, IReadOnlyList<GameAction>, IAgent>? TurnStarted;
    public event Action<string, IAgent>? ValidationFailed; 
    public event Action<GameAction, IPlayer>? ActionExecuted;

    public GameOrchestrator(
        IGameState initialState,
        IReadOnlyDictionary<int, IAgent> agentsBySeat,
        IGameRules rules)
    {
        _initialState = initialState;
        _agentsBySeat = agentsBySeat;
        _rules = rules;
    }

    public IGameState Run()
    {
        IGameState currentState = _initialState.Start();

        while (currentState.GameProgression != GameProgression.GameOver)
        {
            var validActions = _rules.Explore(currentState);
            var activeAgent = _agentsBySeat[currentState.ActivePlayer.Seat];

            TurnStarted?.Invoke(currentState, validActions, activeAgent);

            var command = activeAgent.DecideCommand(currentState, validActions);

            if (!_rules.Validate(command.Action))
            {
                ValidationFailed?.Invoke(command.CommandText, activeAgent);
                continue;
            }

            ActionExecuted?.Invoke(command.Action, currentState.ActivePlayer);
            currentState = _rules.Reduce(currentState, command.Action);
        }

        return currentState;
    }
}