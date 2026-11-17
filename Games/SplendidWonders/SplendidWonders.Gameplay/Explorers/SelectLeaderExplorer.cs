using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;

namespace SplendidWonders.Gameplay.Explorers
{
    public class SelectLeaderExplorer : IActionExplorer<SelectLeaderAction>
    {
        public IReadOnlyList<SelectLeaderAction> Explore(GameState gameState)
        {
            List<SelectLeaderAction> selectLeaderActions = new List<SelectLeaderAction>();

            foreach (var leader in gameState.Leaders)
            {
                Dictionary<DevelopmentColor, int> requirements = new Dictionary<DevelopmentColor, int>()
                {
                    { DevelopmentColor.Yellow, leader.Requirements.Yellow },
                    { DevelopmentColor.Blue, leader.Requirements.Blue },
                    { DevelopmentColor.Green, leader.Requirements.Green },
                    { DevelopmentColor.Brown, leader.Requirements.Brown },
                    { DevelopmentColor.Red, leader.Requirements.Red },
                };

                bool hasFailedRequirement = false;

                foreach (var requirement in requirements)
                {
                    if (requirement.Value == 0)
                        continue;

                    if (gameState.ActivePlayer.Tableau.Count(card => card.Bonus == requirement.Key) < requirement.Value)
                    {
                        hasFailedRequirement = true;
                        break;
                    }
                }

                if (!hasFailedRequirement)
                {
                    selectLeaderActions.Add(new SelectLeaderAction() { Leader = leader });
                }
            }

            return selectLeaderActions;
        }
    }
}
