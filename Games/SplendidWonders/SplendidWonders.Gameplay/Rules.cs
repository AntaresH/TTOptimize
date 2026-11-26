using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using SplendidWonders.Gameplay.Explorers;
using SplendidWonders.Gameplay.Reducers;
using System.Data;
using System.Numerics;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.Setup;

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

        public IReadOnlyList<IPlayerResult> GetFinalStandings(IGameState finalState)
        {
            var swGameState = (GameState)finalState;

            var orderedResults = swGameState.Players
                .Select(p => new PlayerResult()
                {
                    Player = p,
                    Points = p.Tableau.Sum(c => c.VictoryPoints) + p.AcquiredLeaders.Count * 3,
                    Tiebreaker = p.Tableau.Count
                })
                .OrderByDescending(pr => pr.Points)
                .ThenBy(pr => pr.Tiebreaker)
                .ToList();

            var standings = new List<IPlayerResult>(orderedResults.Count);

            for (int i = 0; i < orderedResults.Count; i++)
            {
                var current = orderedResults[i];
                var prev = i > 0 ? orderedResults[i - 1] : null;
                var next = i < orderedResults.Count - 1 ? orderedResults[i + 1] : null;

                bool isExactTieWithPrev = prev != null
                    && current.Points == prev.Points
                    && current.Tiebreaker == prev.Tiebreaker;

                int rank = isExactTieWithPrev ? standings[i - 1].Rank : i + 1;

                bool hasPointsTie = (prev != null && prev.Points == current.Points)
                                 || (next != null && next.Points == current.Points);

                standings.Add(new PlayerResult()
                {
                    Player = current.Player,
                    Rank = rank,
                    Points = current.Points,
                    Tiebreaker = hasPointsTie ? current.Tiebreaker : null
                });
            }

            return standings;
        }
    }
}
