namespace TTOptimize.Core.GameAbstractions;

public interface IActionExplorer
{
    IList<GameAction> Explore(IGameState state);
}
