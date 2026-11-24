using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Engine;

public record class MatchResult
{
    public IGameState FinalState { get; }
    public IReadOnlyList<IPlayerResult> Standings { get; }
    public IReadOnlyList<ActionLogEntry> ActionHistory { get; }
    public TimeSpan Elapsed { get; }

    public MatchResult(
        IGameState finalState,
        IReadOnlyList<IPlayerResult> standings,
        IReadOnlyList<ActionLogEntry> actionHistory,
        TimeSpan elapsed)
    {
        FinalState = finalState;
        Standings = standings;
        ActionHistory = actionHistory;
        Elapsed = elapsed;
    }
}
