namespace TTOptimize.Core.Statistics;

[AttributeUsage(AttributeTargets.Property)]
public sealed class TrackMetricAttribute : Attribute
{
    public string DisplayName { get; }
    public TrackMetricAttribute(string displayName) => DisplayName = displayName;
}

