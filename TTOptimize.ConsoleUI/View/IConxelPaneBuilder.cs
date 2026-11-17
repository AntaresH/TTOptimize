namespace TTOptimize.ConsoleUI.View;

public interface IConxelPaneBuilder<in TData>
{
    ConxelCanvas Build(TData paneData);
}
