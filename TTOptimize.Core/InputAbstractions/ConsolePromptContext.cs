
using TTOptimize.Core;

public class ConsolePromptContext : IConsolePromptContext
{
    private readonly Stack<int> _checkpoints = new();

    public void BeginZone()
    {
        _checkpoints.Push(Console.CursorTop);
    }

    public void ResetZone()
    {
        if (!_checkpoints.TryPeek(out int targetLine)) return;

        Console.SetCursorPosition(0, targetLine);
        Console.Write("\x1b[J");
    }

    public void EndZone()
    {
        if (_checkpoints.Count > 0)
        {
            _checkpoints.Pop();
        }
    }

    public void WriteLine(string text = "") => Console.WriteLine(text);
    public void Write(string text) => Console.Write(text);
}