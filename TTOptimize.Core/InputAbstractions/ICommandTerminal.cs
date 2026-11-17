using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.Core;

public interface ICommandTerminal
{
    event Action<CommandRenderData>? OutputChanged;

    void BeginZone();       
    void ResetZone();    
    void EndZone();        

    void WriteLine(string text = "");
    void Write(string text);
}
