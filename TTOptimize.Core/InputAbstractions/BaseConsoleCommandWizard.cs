using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.InputAbstractions;

public abstract class BaseConsoleCommandWizard : IConsoleCommandWizard
{
    public abstract string CommandKey { get; }
    public abstract string Description { get; }
    public ICommandTerminal _commandTerminal { get; }

    public BaseConsoleCommandWizard(ICommandTerminal commandTerminal)
    {
        _commandTerminal = commandTerminal;
    }

    public abstract bool CanExecute(IGameState state, IEnumerable<GameAction> validActions);
    
    public abstract GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions);

    protected abstract GameAction? ParseInput(string input);

    protected GameCommand PromptForParameter()
    {
        _commandTerminal.BeginZone();

        try
        {
            while (true)
            {
                string? input = ReadLineOrEscape();

                if (input is null)
                {
                    _commandTerminal.ResetZone();
                    throw new ConsoleUserBackException();
                }

                if (string.IsNullOrEmpty(input))
                {
                    _commandTerminal.ResetZone();
                    continue;
                }

                if (input.ToUpper() == "X")
                {
                    _commandTerminal.ResetZone();
                    throw new ConsoleUserAbortException();
                }

                GameAction? generatedAction = ParseInput(input);

                if (generatedAction != null)
                {
                    return new GameCommand()
                    {
                        Action = generatedAction,
                        CommandText = $"{CommandKey} {input}"
                    };
                }
                else
                {
                    _commandTerminal.ResetZone();
                }
            }
        }
        finally
        {
            _commandTerminal.EndZone();
        }
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

public abstract class PresentationWizard : BaseConsoleCommandWizard
{
    protected PresentationWizard(ICommandTerminal commandTerminal) : base(commandTerminal)
    {
    }
}