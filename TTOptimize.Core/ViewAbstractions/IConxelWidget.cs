using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.ViewAbstractions;

public interface IConxelWidget
{
    public int Rows { get; }
    public int Columns { get; }
    ConxelCanvas RenderToBuffer(IGameState state, IReadOnlyList<GameAction> legalActions);
}
