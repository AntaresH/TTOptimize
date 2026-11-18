using TTOptimize.ConsoleUI.View;

namespace TTOptimize.ConsoleUI.Input;

public class ConxelTerminal : ICommandTerminal
{
    private readonly List<TerminalLine> _committedLines = new();
    private readonly Stack<int> _zoneCheckpoints = new();
    private TerminalLine _activeLine = new();

    public event Action<CommandRenderData>? OutputChanged;

    public void BeginZone()
    {
        _zoneCheckpoints.Push(_committedLines.Count);
    }

    public void ResetZone()
    {
        if (!_zoneCheckpoints.TryPeek(out int startIndex)) return;

        if (_committedLines.Count > startIndex)
        {
            _committedLines.RemoveRange(startIndex, _committedLines.Count - startIndex);
        }

        _activeLine = new TerminalLine();
        Flush();
    }

    public void EndZone()
    {
        if (_zoneCheckpoints.Count > 0)
        {
            _zoneCheckpoints.Pop();
        }
    }

    public void Write(string text, RgbColor? foreground = null, RgbColor? background = null)
    {
        if (string.IsNullOrEmpty(text)) return;

        _activeLine.LeftSpans.Add(new TerminalSpan(text, foreground, background));
        if (background != null && _activeLine.LineBackground == null)
        {
            _activeLine.LineBackground = background;
        }
    }

    public void WriteRight(string text, RgbColor? foreground = null, RgbColor? background = null)
    {
        if (string.IsNullOrEmpty(text)) return;

        _activeLine.RightSpans.Add(new TerminalSpan(text, foreground, background));
        if (background != null && _activeLine.LineBackground == null)
        {
            _activeLine.LineBackground = background;
        }
    }

    public void WriteLine(string text = "", RgbColor? foreground = null, RgbColor? background = null)
    {
        if (!string.IsNullOrEmpty(text))
        {
            _activeLine.LeftSpans.Add(new TerminalSpan(text, foreground, background));
            if (background != null && _activeLine.LineBackground == null)
            {
                _activeLine.LineBackground = background;
            }
        }

        _committedLines.Add(_activeLine);
        _activeLine = new TerminalLine();
    }

    public void WriteDivider(RgbColor? foreground = null, RgbColor? background = null)
    {
        if (_activeLine.LeftSpans.Count > 0 || _activeLine.RightSpans.Count > 0)
        {
            _committedLines.Add(_activeLine);
            _activeLine = new TerminalLine();
        }

        _committedLines.Add(new TerminalLine
        {
            IsDivider = true,
            DividerForeground = foreground,
            LineBackground = background
        });
    }

    public void Flush()
    {
        var allLines = new List<TerminalLine>(_committedLines);

        if (_activeLine.LeftSpans.Count > 0 || _activeLine.RightSpans.Count > 0)
        {
            allLines.Add(_activeLine);
        }

        OutputChanged?.Invoke(new CommandRenderData(allLines));
    }
}
