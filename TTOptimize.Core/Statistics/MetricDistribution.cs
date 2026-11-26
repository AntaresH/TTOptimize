namespace TTOptimize.Core.Statistics;

public class MetricDistribution
{
    public long SampleCount { get; set; }
    public double Mean { get; set; }
    public double M2 { get; set; }

    public double Variance => SampleCount > 1 ? M2 / (SampleCount - 1) : 0.0;
    public double StandardDeviation => Math.Sqrt(Variance);

    public void AddObservation(double value)
    {
        SampleCount++;
        double delta = value - Mean;
        Mean += delta / SampleCount;
        double delta2 = value - Mean;
        M2 += delta * delta2;
    }
}