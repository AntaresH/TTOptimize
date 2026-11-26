using TTOptimize.Core.Agents;

namespace TTOptimize.Core.GameAbstractions;

public record class ActionLogEntry
{
    public int TurnNumber { get; set; }
    public IPlayerState Player { get; set; }

    public GameAction Action { get; set; }
}
