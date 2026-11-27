namespace TTOptimize.Core.Statistics;

public class RunningMetric
{
    public int Count { get; set; }
    public double Mean { get; set; }
    public double M2 { get; set; }
    public double Min { get; set; } = double.MaxValue;
    public double Max { get; set; } = double.MinValue;

    public double Variance => Count > 1 ? M2 / (Count - 1) : 0.0;
    public double StandardDeviation => Math.Sqrt(Variance);

    public void Track(double value)
    {
        Count++;
        if (value < Min) Min = value;
        if (value > Max) Max = value;

        double delta = value - Mean;
        Mean += delta / Count;
        double delta2 = value - Mean;
        M2 += delta * delta2;
    }
}
