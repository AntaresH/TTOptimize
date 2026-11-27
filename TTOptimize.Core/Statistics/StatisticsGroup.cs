using TTOptimize.Core.Statistics;

public class StatisticsGroup
{
    public string Id
    {
        get => Key.ToStatisticsGroupId();
        set { } 
    }

    public AggregationKey Key { get; set; } = default!;
    public int SampleCount { get; set; } 

    public RunningMetric VictoryPoints { get; set; } = new();
    public RunningMetric TurnCount { get; set; } = new();
    public Dictionary<string, RunningMetric> ActionRatios { get; set; } = new();

    // A [TrackMetric] attribútummal jelölt játék-specifikus metrikák (pl. "TableauSize", "AcquiredLeadersCount")
    public Dictionary<string, RunningMetric> CustomMetrics { get; set; } = new();
}