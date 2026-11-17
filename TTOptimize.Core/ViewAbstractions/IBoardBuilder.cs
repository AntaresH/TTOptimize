using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.ViewAbstractions;

public interface IPaneBuilder<in TData>
{
    ConxelCanvas Build(TData paneData);
}
