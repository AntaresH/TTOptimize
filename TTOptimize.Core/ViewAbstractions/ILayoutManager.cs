using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.ViewAbstractions;

public interface ILayoutManager
{
    ConxelCanvas BuildCanvas(IGameState state, IReadOnlyList<GameAction>? legalActions, string? errorMessage);
}
