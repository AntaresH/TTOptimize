using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using TTOptimize.ConsoleUI.Input;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input;

public class SplendidWondersInputPresenter : ISplendidWondersInputPresenter
{
    private static RgbColor _commandsBlockForeground = new RgbColor(255, 255, 255);
    private static RgbColor _activeCommandForeground = new RgbColor(255, 255, 50);
    private static RgbColor _escCommandForeground = new RgbColor(255, 200, 200);
    private static RgbColor _presentationCommandForeground = new RgbColor(180, 200, 170);
    private static RgbColor _commandsBlockBackground = new RgbColor(50, 50, 50);

    private readonly ICommandTerminal _commandTerminal;

    public SplendidWondersInputPresenter(ICommandTerminal commandTerminal)
    {
        _commandTerminal = commandTerminal;
    }

    public void DisplayCommandBlock(IEnumerable<ITerminalWizard> wizards, ITerminalWizard activeWizard)
    {
        _commandTerminal.WriteDivider(_commandsBlockForeground, _commandsBlockBackground);

        _commandTerminal.Write(" " + Strings.ValidCommands, _commandsBlockForeground, _commandsBlockBackground);
        _commandTerminal.WriteRight($"X - {Strings.QuitGame}  ", _escCommandForeground, _commandsBlockBackground);
        _commandTerminal.SetActiveLineBackground(_commandsBlockBackground);
        _commandTerminal.WriteLine();

        foreach (var wizard in wizards)
        {
            PrintCommand(wizard, activeWizard == wizard);
        }

        _commandTerminal.WriteDivider(_commandsBlockForeground, _commandsBlockBackground);
    }

    public void DisplaySubCommandBlock(string commandKey, string subCommand)
    {
        _commandTerminal.Write($" | {commandKey} |", _activeCommandForeground, _commandsBlockBackground);
        _commandTerminal.Write($" - {subCommand}", _commandsBlockForeground, _commandsBlockBackground);
        _commandTerminal.WriteRight($"Esc - {Strings.EscapeToMainCommand}  ", _escCommandForeground, _commandsBlockBackground);
        _commandTerminal.SetActiveLineBackground(_commandsBlockBackground);
        _commandTerminal.WriteLine();

        _commandTerminal.WriteDivider(_commandsBlockForeground, _commandsBlockBackground);
    }

    public void DisplayCards(IEnumerable<DevelopmentCard> cards)
    {
        foreach (var card in cards)
        {
            PrintCardWithCosts(card);
        }
    }

    public void DisplayPurchases(IEnumerable<PurchaseAction> purchaseActions)
    {
        int key = 1;
        foreach (var purchase in purchaseActions)
        {
            PrintCardWithSpendingInfo(key++, purchase.Card, purchase.RequiredTokens);
        }
    }

    public void DisplayLeaders(IEnumerable<Leader> leaders)
    {
        foreach (var leader in leaders)
        {
            PrintLeader(leader);
        }
    }

    public void DisplayValidationFailure(string validationFailureMessage)
    {
        _commandTerminal.WriteDivider(ColorHelper.LightSetYellow, ColorHelper.DarkRed);
        _commandTerminal.SetActiveLineBackground(ColorHelper.DarkRed);
        _commandTerminal.WriteLine(validationFailureMessage, ColorHelper.LightSetYellow, ColorHelper.DarkRed);
        _commandTerminal.WriteDivider(ColorHelper.LightSetYellow, ColorHelper.DarkRed);
    }

    private void PrintCommand(ITerminalWizard wizard, bool isActive)
    {
        var fgColor = isActive 
            ? _activeCommandForeground : 
                wizard is BasePresentationWizard 
                ? _presentationCommandForeground 
                : _commandsBlockForeground;

        _commandTerminal.SetActiveLineBackground(_commandsBlockBackground);
        _commandTerminal.WriteLine($"  {wizard.CommandKey} - {wizard.Description}", fgColor, _commandsBlockBackground);
    }

    private void PrintLeader(Leader leader)
    {
        string namePadding = new string(' ', Math.Max(0, 20 - leader.Name.Length));

        _commandTerminal.Write($"{leader.Id,-8}{leader.Name}{namePadding}");

        var conditionList = leader.Requirements.AsEnumerable().Where(dc => dc.Value > 0);

        foreach (var condition in conditionList)
        {
            _commandTerminal.Write($" {condition.Value} ", ColorHelper.FullWhite, ColorHelper.DarkSetColor(condition.Key));
            _commandTerminal.Write($" ");
        }

        _commandTerminal.WriteLine();
    }

    private void PrintCardWithCosts(DevelopmentCard card)
    {
        string vpDisplay = card.VictoryPoints > 0 ? $"{{{card.VictoryPoints}}}" : "   ";
        string namePadding = new string(' ', Math.Max(0, 26 - card.Name.Length));

        _commandTerminal.Write($"{card.Id,-5} ");
        _commandTerminal.Write(card.Name, ColorHelper.LightSetColor(card.Bonus));
        _commandTerminal.Write($"{namePadding}   {vpDisplay}   ");

        var conditionList = card.Cost.AsEnumerable().Where(dc => dc.Value > 0 && dc.Key != DevelopmentColor.Polymath);

        foreach (var condition in conditionList)
        {
            _commandTerminal.Write($"{condition.Value} ", ColorHelper.LightSetColor(condition.Key));
        }

        _commandTerminal.WriteLine();
    }

    private void PrintCardWithSpendingInfo(int key, DevelopmentCard card, TokenGroup requiredTokens)
    {
        string vpDisplay = card.VictoryPoints > 0 ? $"{{{card.VictoryPoints}}}" : "   ";
        string namePadding = new string(' ', Math.Max(0, 26 - card.Name.Length));
        
        _commandTerminal.Write($"{key, -5} ");
        _commandTerminal.Write(card.Name, ColorHelper.LightSetColor(card.Bonus));
        _commandTerminal.Write($"{namePadding}   {vpDisplay}   ");

        var conditionList = requiredTokens.AsEnumerable().Where(dc => dc.Value > 0);

        if (!conditionList.Any())
            _commandTerminal.Write(Strings.FreePurchase);

        foreach (var condition in conditionList.Where(dc => dc.Key != DevelopmentColor.Polymath))
        {
            _commandTerminal.Write($"{condition.Value} ", ColorHelper.LightSetColor(condition.Key));
        }

        if (requiredTokens.Polymath > 0)
        {
            string costPadding = new string(' ', Math.Max(0, 10 - conditionList.Count() * 2));
            string polymaths = new string('*', requiredTokens.Polymath);
            _commandTerminal.Write($"{costPadding}{polymaths}", ColorHelper.FullWhite);
        }

        _commandTerminal.WriteLine();
    }
}
