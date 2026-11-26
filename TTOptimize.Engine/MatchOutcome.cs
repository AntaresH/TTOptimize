using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Engine;

public record class MatchOutcome
{
    public IGameState FinalState { get; }
    public IReadOnlyList<IPlayerResult> Standings { get; }
    public IReadOnlyList<ActionLogEntry> ActionHistory { get; }

    public MatchOutcome(
        IGameState finalState,
        IReadOnlyList<IPlayerResult> standings,
        IReadOnlyList<ActionLogEntry> actionHistory)
    {
        FinalState = finalState;
        Standings = standings;
        ActionHistory = actionHistory;
    }
}
