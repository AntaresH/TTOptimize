namespace TTOptimize.Core.Statistics;

public record class MatchRecord
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string GameId { get; init; }
    public Guid? ExperimentId { get; init; } // Ha egy 10.000-es batch futás része
    public DateTime PlayedAtUtc { get; init; } = DateTime.UtcNow;
    public TimeSpan Duration { get; init; }
    public string GameOverReason { get; init; } = string.Empty;

    // A meccsben használt játék-konfiguráció (későbbi játéktér optimalizáláshoz)
    public required object GameConfiguration { get; init; }

    // Játékosonkénti / Ágensenkénti eredmények és nyers statisztikák
    public required List<IPlayerMetrics> PlayerMetrics { get; init; }

    // Jövőbeli 2. fázishoz: ágens-független, játékkomponens-szintű adatok
    // Pl. "BoughtCardIds":, "ClaimedLeaderIds": [2, 5]
    public required IGameMetrics GameMetrics { get; init; }
}