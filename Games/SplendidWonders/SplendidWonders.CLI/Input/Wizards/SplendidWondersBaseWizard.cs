using TTOptimize.ConsoleUI;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input.Wizards;

public abstract class SplendidWondersBaseWizard : BaseTerminalWizard
{
    protected ISplendidWondersInputPresenter _inputPresenter;

    protected SplendidWondersBaseWizard(ICommandTerminal commandTerminal, ISplendidWondersInputPresenter inputPresenter) : base(commandTerminal)
    {
        _inputPresenter = inputPresenter;
    }
}
