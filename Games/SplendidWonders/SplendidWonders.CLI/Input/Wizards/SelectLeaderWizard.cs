using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using System.Text.RegularExpressions;
using TTOptimize.ConsoleUI.Input;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.Setup;

namespace SplendidWonders.CLI.Input.Wizards;

public class SelectLeaderWizard : SplendidWondersBaseWizard
{
    public override string CommandKey => Strings.SelectLeaderCommandKey;

    public override string Description => Strings.SelectLeaderCommandDescriptionFormat;

    private IEnumerable<Leader> _eligibleLeaders;

    public override bool CanExecute(IGameState state, IEnumerable<GameAction> validActions)
        => validActions.OfType<SelectLeaderAction>().Any();

    public SelectLeaderWizard(
        ICommandTerminal commandTerminal,
        ISplendidWondersInputPresenter inputPresenter) : base(commandTerminal, inputPresenter)
    {
        _inputPresenter = inputPresenter;
    }

    public override GameCommand? RunConsoleDialog(IGameState state, IEnumerable<GameAction> validActions)
    {
        _eligibleLeaders = validActions.OfType<SelectLeaderAction>().Select(a => a.Leader);

        _inputPresenter.DisplaySubCommandBlock(CommandKey, Strings.PromptSelectLeader);
        _inputPresenter.DisplayLeaders(_eligibleLeaders);

        return PromptForParameter();
    }

    protected override GameAction? ParseInput(string input)
    {
        Match match = Regex.Match(input, @"^#?([\d]){1,2}$");

        if (match.Success)
        {
            int leaderId = int.Parse(match.Groups[0].Value);

            if (_eligibleLeaders.Any(c => c.Id == leaderId))
            {
                return new SelectLeaderAction()
                {
                    Leader = _eligibleLeaders.First(c => c.Id == leaderId)
                };
            }
        }

        return null;
    }
}
