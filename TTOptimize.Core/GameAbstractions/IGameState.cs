namespace TTOptimize.Core.GameAbstractions;

public interface IGameState
{
    IGameState Start();

    GameOverReason GameOverReason { get; }
    GameProgression GameProgression { get; }

    IReadOnlyList<IPlayer> Players { get; }
    IPlayer ActivePlayer { get; }
}
