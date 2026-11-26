namespace TTOptimize.Core.Statistics;

public abstract record class PlayerGameMetrics
{
    public int PlayerId { get; init; }
    public required string AgentName { get; init; }
    public required string AgentArchetype { get; init; }

    public int Rank { get; init; }
    public bool IsWinner { get; init; }
    public int VictoryPoints { get; init; }

    public int TurnCount { get; init; }
    public Dictionary<string, int> ActionCounts { get; init; } = new();
    public Dictionary<string, double> ActionRatios { get; init; } = new();
}
