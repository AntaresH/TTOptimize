namespace TTOptimize.Core.GameAbstractions;

public interface IGameState
{
    GameProgression GameProgression { get; }

    int ActivePlayerId { get; }
}
