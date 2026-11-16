using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.InputAbstractions;

public abstract class BaseConsoleCommandWizard : IConsoleCommandWizard
{
    public abstract string CommandKey { get; }
    public abstract string Description { get; }

    private int _promptStartTop;

    public abstract bool CanExecute(IGameState state, IEnumerable<GameAction> validActions);

    public GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions, int promptStartTop)
    {
        _promptStartTop = promptStartTop;

        return RunConsoleDialog(state, validActions);
    }

    public abstract GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions);

    protected abstract GameAction? ParseInput(string input);

    protected GameCommand PromptForParameter()
    {
        int promptStartTop = Console.CursorTop;

        while (true)
        {
            ClearFromLine(promptStartTop);

            string? input = ReadLineOrEscape();

            if (input is null)
            {
                throw new ConsoleUserBackException();
            }

            if (string.IsNullOrEmpty(input))
                continue;

            if (input.ToUpper() == "X")
                throw new ConsoleUserAbortException();

            GameAction? generatedAction = ParseInput(input);

            if (generatedAction != null)
            {
                return new GameCommand()
                {
                    Action = generatedAction,
                    CommandText = $"{CommandKey} {input}"
                };
            }
        }
    }

    protected void ClearFromLine(int startLine)
    {
        Console.SetCursorPosition(0, startLine);
        Console.Write("\x1b[J");
    }

    public string? ReadLineOrEscape()
    {
        var buffer = new System.Text.StringBuilder();

        while (true)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

            if (keyInfo.Key == ConsoleKey.Escape)
            {
                Console.WriteLine(); 
                return null;         
            }

            if (keyInfo.Key == ConsoleKey.Enter)
            {
                Console.WriteLine();
                return buffer.ToString();
            }

            if (keyInfo.Key == ConsoleKey.Backspace)
            {
                if (buffer.Length > 0)
                {
                    buffer.Length--;
                    Console.Write("\b \b"); 
                }
            }
            else if (!char.IsControl(keyInfo.KeyChar))
            {
                buffer.Append(keyInfo.KeyChar);
                Console.Write(keyInfo.KeyChar);
            }
        }
    }
}

public abstract class PresentationWizard : BaseConsoleCommandWizard { }