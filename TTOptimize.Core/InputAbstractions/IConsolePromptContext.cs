namespace TTOptimize.Core;

public interface IConsolePromptContext
{
    void BeginZone();       
    void ResetZone();    
    void EndZone();        

    void WriteLine(string text = "");
    void Write(string text);
}
