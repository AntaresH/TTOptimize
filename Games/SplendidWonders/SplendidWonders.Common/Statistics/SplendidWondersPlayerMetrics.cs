using TTOptimize.Core.Statistics;

namespace SplendidWonders.Common.Statistics;

public sealed record class SplendidWondersPlayerMetrics : BasePlayerMetrics
{
    [TrackMetric("TableauSize")]
    public int TableauSize { get; init; }

    [TrackMetric("AcquiredLeadersCount")]
    public int AcquiredLeadersCount { get; init; }

    [TrackMetric("RemainingReservedCards")]
    public int RemainingReservedCards { get; init; }

    [TrackMetric("RemaningTokens")]
    public int RemainingTokens { get; init; }

    public SplendidWondersPlayerMetrics(IPlayerMetrics baseMetrics) : base(baseMetrics)
    {
    }
}
