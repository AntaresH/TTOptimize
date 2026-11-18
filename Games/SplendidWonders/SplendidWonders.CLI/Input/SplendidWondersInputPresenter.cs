using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
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

    private void PrintCommand(ITerminalWizard wizard, bool isActive)
    {
        _commandTerminal.WriteLine($"  {wizard.CommandKey} - {wizard.Description}", _presentationCommandForeground, _commandsBlockBackground);
    }

    private void PrintLeader(Leader leader)
    {
        List<string> colorCostTexts = new List<string>();

        if (leader.Requirements.Yellow > 0)
        {
            colorCostTexts.Add(GetColorizedConsoleText($" {leader.Requirements.Yellow} ", ColorHelper.FullWhite, ColorHelper.DarkSetColor(DevelopmentColor.Yellow)));
        }

        if (leader.Requirements.Blue > 0)
        {
            colorCostTexts.Add(GetColorizedConsoleText($" {leader.Requirements.Blue} ", ColorHelper.FullWhite, ColorHelper.DarkSetColor(DevelopmentColor.Blue)));
        }

        if (leader.Requirements.Green > 0)
        {
            colorCostTexts.Add(GetColorizedConsoleText($" {leader.Requirements.Green} ", ColorHelper.FullWhite, ColorHelper.DarkSetColor(DevelopmentColor.Green)));
        }

        if (leader.Requirements.Brown > 0)
        {
            colorCostTexts.Add(GetColorizedConsoleText($" {leader.Requirements.Brown} ", ColorHelper.FullWhite, ColorHelper.DarkSetColor(DevelopmentColor.Brown)));
        }

        if (leader.Requirements.Red > 0)
        {
            colorCostTexts.Add(GetColorizedConsoleText($" {leader.Requirements.Red} ", ColorHelper.FullWhite, ColorHelper.DarkSetColor(DevelopmentColor.Red)));
        }

        string namePadding = new string(' ', Math.Max(0, 18 - leader.Name.Length));
        string colorCost = string.Join(" ", colorCostTexts);

        _commandTerminal.WriteLine($"{leader.Id,-5} {leader.Name}{namePadding} Feltételek: {colorCost}");
    }

    private void PrintCardWithCosts(DevelopmentCard card)
    {
        List<string> colorCostSubTexts = new List<string>();

        if (card.Cost.Yellow > 0)
        {
            colorCostSubTexts.Add(GetColorizedConsoleText(card.Cost.Yellow, ColorHelper.LightSetYellow));
        }

        if (card.Cost.Blue > 0)
        {
            colorCostSubTexts.Add(GetColorizedConsoleText(card.Cost.Blue, ColorHelper.LightSetBlue));
        }

        if (card.Cost.Green > 0)
        {
            colorCostSubTexts.Add(GetColorizedConsoleText(card.Cost.Green, ColorHelper.LightSetGreen));
        }

        if (card.Cost.Brown > 0)
        {
            colorCostSubTexts.Add(GetColorizedConsoleText(card.Cost.Brown, ColorHelper.LightSetBrown));
        }

        if (card.Cost.Red > 0)
        {
            colorCostSubTexts.Add(GetColorizedConsoleText(card.Cost.Red, ColorHelper.LightSetRed));
        }

        string vpDisplay = card.VictoryPoints > 0 ? $"{{{card.VictoryPoints}}}" : "   ";

        string namePadding = new string(' ', Math.Max(0, 26 - card.Name.Length));
        string coloredName = GetColorizedConsoleText(card.Name, ColorHelper.LightSetColor(card.Bonus));

        string colorCost = string.Join(" ", colorCostSubTexts);
        string costPadding = new string(' ', Math.Max(0, 12 - colorCostSubTexts.Count * 2 - 1));

        _commandTerminal.WriteLine($"{card.Id,-5} {coloredName}{namePadding}   {vpDisplay}   {colorCost}{costPadding}");
    }

    private void PrintCardWithSpendingInfo(int key, DevelopmentCard card, TokenGroup requiredTokens)
    {
        List<string> colorCostSubTexts = new List<string>();

        if (requiredTokens.Yellow > 0)
        {
            colorCostSubTexts.Add(GetColorizedConsoleText(requiredTokens.Yellow, ColorHelper.LightSetYellow));
        }

        if (requiredTokens.Blue > 0)
        {
            colorCostSubTexts.Add(GetColorizedConsoleText(requiredTokens.Blue, ColorHelper.LightSetBlue));
        }

        if (requiredTokens.Green > 0)
        {
            colorCostSubTexts.Add(GetColorizedConsoleText(requiredTokens.Green, ColorHelper.LightSetGreen));
        }

        if (requiredTokens.Brown > 0)
        {
            colorCostSubTexts.Add(GetColorizedConsoleText(requiredTokens.Brown, ColorHelper.LightSetBrown));
        }

        if (requiredTokens.Red > 0)
        {
            colorCostSubTexts.Add(GetColorizedConsoleText(requiredTokens.Red, ColorHelper.LightSetRed));
        }

        string vpDisplay = card.VictoryPoints > 0 ? $"{{{card.VictoryPoints}}}" : "   ";

        string namePadding = new string(' ', Math.Max(0, 26 - card.Name.Length));
        string coloredName = GetColorizedConsoleText(card.Name, ColorHelper.LightSetColor(card.Bonus));

        string colorCost = string.Join(" ", colorCostSubTexts);
        string costPadding = new string(' ', Math.Max(0, 12 - colorCostSubTexts.Count * 2 - 1));
        string polymathCost = GetColorizedConsoleText(new string('*', requiredTokens.Polymath), ColorHelper.FullWhite);

        string completeCostText = colorCostSubTexts.Any() || requiredTokens.Polymath > 0
            ? $"{colorCost}{costPadding}{polymathCost}"
            : Strings.FreePurchase;

        _commandTerminal.WriteLine($"{key,-5} {coloredName}{namePadding}   {vpDisplay}   {completeCostText}");
    }

    private string GetColorizedConsoleText(object text, RgbColor? foreground = null, RgbColor? background = null)
    {
        string foreGroundColorsRgb = ColorHelper.GetRgbColorString(foreground.HasValue ? foreground.Value : ColorHelper.DefaultConsoleForeground);
        string backGroundColorsRgb = ColorHelper.GetRgbColorString(background.HasValue ? background.Value : ColorHelper.DefaultConsoleBackground);

        string fgColorPrefix = foreground.HasValue ? $"\u001b[38;2;{foreGroundColorsRgb}m" : string.Empty;
        string fgColorSuffix = foreground.HasValue ? "\u001b[39m" : string.Empty;

        string bgColorPrefix = background.HasValue ? $"\u001b[48;2;{backGroundColorsRgb}m" : string.Empty;
        string bgColorSuffix = background.HasValue ? "\u001b[49m" : string.Empty;

        return $"{fgColorPrefix}{bgColorPrefix}{text.ToString()}{bgColorSuffix}{fgColorSuffix}";
    }
}
