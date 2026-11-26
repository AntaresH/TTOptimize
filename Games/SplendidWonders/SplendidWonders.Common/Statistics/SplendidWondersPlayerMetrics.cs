using TTOptimize.Core.Statistics;

namespace SplendidWonders.Common.Statistics;

public sealed record class SplendidWondersPlayerMetrics : BasePlayerMetrics
{
    [TrackMetric("Tabló mérete")]
    public int TableauSize { get; init; }

    [TrackMetric("Megszerzett vezére száma")]
    public int AcquiredLeadersCount { get; init; }

    [TrackMetric("Kézben maradt foglalt kártyák száma")]
    public int RemainingReservedCards { get; init; }

    [TrackMetric("Fel nem használt fejlődéslapkák száma")]
    public int RemainingTokens { get; init; }

    public SplendidWondersPlayerMetrics(IPlayerMetrics baseMetrics) : base(baseMetrics)
    {
    }
}
