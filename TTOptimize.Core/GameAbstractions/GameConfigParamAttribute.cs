namespace TTOptimize.Core;

[AttributeUsage(AttributeTargets.Property)]
public class GameConfigParamAttribute : Attribute
{
    public string DisplayName { get; }
    public int Min { get; }
    public int Max { get; }

    public GameConfigParamAttribute(string displayName, int min = 0, int max = int.MaxValue)
    {
        DisplayName = displayName;
        Min = min;
        Max = max;
    }
}