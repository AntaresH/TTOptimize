using TTOptimize.Core;
using TTOptimize.Core.ViewAbstractions;

public class ConxelCommandTerminal : ICommandTerminal
{
    private readonly List<string> _lines = new();
    private readonly Stack<int> _zoneStartIndices = new();
    private string _currentLineBuffer = string.Empty;

    public event Action<CommandRenderData>? OutputChanged;

    public ConxelCommandTerminal()
    {
    }

    public void BeginZone()
    {
        _zoneStartIndices.Push(_lines.Count);
    }

    public void ResetZone()
    {
        if (!_zoneStartIndices.TryPeek(out int startIndex)) return;

        if (_lines.Count > startIndex)
        {
            _lines.RemoveRange(startIndex, _lines.Count - startIndex);
        }

        _currentLineBuffer = string.Empty;
        FlushToView();
    }

    public void EndZone()
    {
        if (_zoneStartIndices.Count > 0)
        {
            _zoneStartIndices.Pop();
        }
    }

    public void Write(string text)
    {
        _currentLineBuffer += text;
        FlushToView();
    }

    public void WriteLine(string text = "")
    {
        _lines.Add(_currentLineBuffer + text);
        _currentLineBuffer = string.Empty;
        FlushToView();
    }

    public string? ReadLine()
    {
        string? input = Console.ReadLine();
        WriteLine(input ?? string.Empty);
        return input;
    }

    private void FlushToView()
    {
        var renderLines = new List<string>(_lines);
        if (!string.IsNullOrEmpty(_currentLineBuffer))
        {
            renderLines.Add(_currentLineBuffer);
        }

        OutputChanged?.Invoke(new CommandRenderData(_lines));
    }
}