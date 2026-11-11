using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using SplendidWonders.Gameplay.Actions;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using System.Data;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Gameplay
{
    public class Rules : IGameRules
    {
        IList<GameAction> _validActions;
        private readonly IActionExplorer _actionExplorer;
        private readonly IStateReducer _stateReducer;

        public Rules(
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IActionExplorer actionExplorer,
            [FromKeyedServices(Games.SPLENDID_WONDERS_KEY)] IStateReducer stateReducer)
        {
            _actionExplorer = actionExplorer;
            _stateReducer = stateReducer;
        }

        public bool Validate(GameAction command)
        {
            return _validActions.Any(action => action.Equals(command));
        }

        public IReadOnlyList<GameAction> Explore(IGameState currentState)
        {
            if (_validActions == null || !_validActions.Any())
            {
                _validActions = _actionExplorer.Explore(currentState);
            }

            return _validActions.ToArray();
        }

        public IGameState Reduce(IGameState baseState, GameAction actionToApply)
        {
            _validActions.Clear();
            return _stateReducer.Reduce(baseState, actionToApply);
        }

        object IGameRules.GetFinalStandings(IGameState gameState)
        {
            var swGameState = (GameState)gameState;

            return swGameState.Players
                .OrderByDescending(p => p.Tableau.Sum(c => c.VictoryPoints) + p.AcquiredLeaders.Count * 3)
                .ThenBy(p => p.Tableau.Count)
                .ToList();
        }
    }
}
