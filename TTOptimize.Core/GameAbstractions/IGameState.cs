namespace TTOptimize.Core.GameAbstractions;

public interface IGameState
{
    IGameState Start();

    GameOverReason GameOverReason { get; }
    GameProgression GameProgression { get; }

    int ActivePlayerId { get; }
}
