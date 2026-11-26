namespace TTOptimize.Core.Statistics;

public record class MatchRecord
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string GameId { get; init; }
    public Guid? BatchRunId { get; init; }
    public string GameOverReason { get; init; } = string.Empty;

    public required object GameConfiguration { get; init; }

    public required List<IPlayerMetrics> PlayerMetrics { get; init; }

    public required IGameMetrics GameMetrics { get; init; }
}