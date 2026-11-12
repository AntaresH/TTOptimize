using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.InputAbstractions;

public abstract class BaseConsoleCommandWizard : IConsoleCommandWizard
{
    public abstract string CommandKey { get; }
    public abstract string Description { get; }

    public abstract bool CanExecute(IGameState state, IEnumerable<GameAction> validActions);

    public abstract GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions);

    protected abstract GameAction? ParseInput(string input);

    protected GameCommand PromptForParameter()
    {
        while (true)
        {
            string input = (Console.ReadLine() ?? "").Trim();

            if (string.IsNullOrEmpty(input)) 
                continue;

            if (input.ToUpper() == "X") 
                throw new ConsoleUserAbortException();

            if (input.ToUpper() == "V") 
                throw new ConsoleUserBackException();

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
}

public abstract class PresentationWizard : BaseConsoleCommandWizard { }