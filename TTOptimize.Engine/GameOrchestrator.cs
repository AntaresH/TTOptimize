using Microsoft.VisualBasic;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Engine;

public class GameOrchestrator
{
    private const string _invalidCommandType = "A megadott parancs ({0}) ezekkel a paraméterekkel nem végrehajtható a játék jelenlegi állapotában!";

    private readonly IGameConfiguration _gameConfiguration;
    private readonly IReadOnlyList<IPlayerAgent> _playerAgents;
    private readonly IGameStateFactory _factory;
    private readonly IGameRules _rules;
    private readonly IGameView _view;

    public GameOrchestrator(
        IGameConfiguration gameConfiguration,
        IReadOnlyList<IPlayerAgent> playerAgents,
        IGameStateFactory factory,
        IGameRules rules,
        IGameView view)
    {
        _gameConfiguration = gameConfiguration;
        _playerAgents = playerAgents;
        _factory = factory;
        _rules = rules;
        _view = view;
    }

    public void Run()
    {
        IGameState currentState = _factory.Create(_gameConfiguration);

        string validationFailureMessage = string.Empty;

        while (currentState.GameProgression != GameProgression.GameOver)
        {
            var validActions = _rules.Explore(currentState);

            IPlayerAgent activeAgent = _playerAgents[currentState.ActivePlayerId - 1];

            if (activeAgent is HumanAgent)
            {
                _view.Render(currentState, validActions, validationFailureMessage);
                validationFailureMessage = string.Empty;
            }

            var command = activeAgent.DecideCommand(currentState, validActions);

            if (_rules.Validate(command.Action))
            {
                currentState = _rules.Reduce(currentState, command.Action);
            }
            else
            {
                validationFailureMessage = string.Format(_invalidCommandType, command.CommandText);
            }
        }

        var finalStandings = _rules.GetFinalStandings(currentState);
        _view.RenderFinalState(currentState, finalStandings);
    }
}