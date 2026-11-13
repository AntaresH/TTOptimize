using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.ViewAbstractions;

public interface IConxelWidget
{
    ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions);
}
