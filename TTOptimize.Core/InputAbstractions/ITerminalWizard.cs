using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.InputAbstractions;

public interface ITerminalWizard
{
    string CommandKey { get; }
    string Description { get; }

    bool CanExecute(IGameState state, IEnumerable<GameAction> validActions);

    GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions);
}