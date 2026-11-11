using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;
using TTOptimize.Core.GameAbstractions;

namespace SplendidWonders.Gameplay.Explorers
{
    public interface IActionExplorer<T> where T : GameAction
    {
        IEnumerable<T> Explore(GameState state);
    }
}