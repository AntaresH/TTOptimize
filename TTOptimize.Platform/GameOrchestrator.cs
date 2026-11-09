using Microsoft.VisualBasic;
using TTOptimize.Core;

namespace TTOptimize.Platform;

public class GameOrchestrator
{
    private const string _invalidCommandType = "A megadott parancs ({0}) ezekkel a paraméterekkel nem végrehajtható a játék jelenlegi állapotában!";

    private readonly IGameConfiguration _gameConfiguration;
    private readonly IGameStateFactory _factory;
    private readonly IList<IPlayerAgent> _agents;
    private readonly IGameRules _rules;
    private readonly IGameView _view;

    public GameOrchestrator(
        IGameConfiguration gameConfiguration,
        IList<IPlayerAgent> agents,
        IGameStateFactory factory,
        IGameRules rules,
        IGameView view)
    {
        _gameConfiguration = gameConfiguration;
        _agents = agents;
        _factory = factory;
        _rules = rules;
        _view = view;
    }

    public void Run()
    {
        IGameState currentState = _factory.Create(_gameConfiguration);

        string commandMessage = string.Empty;

        while (currentState.GameProgression != GameProgression.GameOver)
        {
            var legalActions = _rules.Explore(currentState);

            _view.Render(currentState, legalActions, commandMessage);
            commandMessage = null;

            IPlayerAgent activeAgent = _agents[currentState.ActivePlayerId - 1];

            var command = activeAgent.DecideCommand(currentState, legalActions);

            if (command is PresentationCommand presentationCommand)
            {
                _view.DisplayPresentationCommand(currentState, presentationCommand);
                continue;
            }
            else if (command is GameCommand gameCommand)
            {
                if (_rules.Validate(gameCommand.Action))
                {
                    currentState = _rules.Reduce(currentState, gameCommand.Action);
                }
                else
                {
                    commandMessage = string.Format(_invalidCommandType, command.CommandText);
                }
            }
        }

        _view.RenderFinalState(currentState);
    }
}