namespace TTOptimize.Core.ViewAbstractions;

public interface IMasterLayout
{
    IReadOnlyList<ScreenPane> CreatePanes();
}
