using TTOptimize.ConsoleUI.Input;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input.Wizards;

public abstract class SplendidWondersPresentationWizard : BasePresentationWizard
{
    protected ISplendidWondersInputPresenter _inputPresenter;

    protected SplendidWondersPresentationWizard(
        ICommandTerminal commandTerminal,
        ISplendidWondersInputPresenter inputPresenter
        ) : base(commandTerminal)
    {
        _inputPresenter = inputPresenter;
    }
}