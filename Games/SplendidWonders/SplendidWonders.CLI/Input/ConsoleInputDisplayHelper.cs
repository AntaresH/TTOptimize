using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using SplendidWonders.Gameplay.Actions;
using System.Runtime.CompilerServices;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.Input
{
    public static class ConsoleInputDisplayHelper
    {
        private static RgbColor _defaultForeground = new RgbColor(230, 225, 205);
        private static RgbColor _defaultBackground = new RgbColor(0, 0, 0);

        private static RgbColor _commandsBlockForeground = new RgbColor(255, 255, 255);
        private static RgbColor _activeCommandForeground = new RgbColor(255, 255, 50);
        private static RgbColor _escCommandForeground = new RgbColor(255, 200, 200);
        private static RgbColor _presentationCommandForeground = new RgbColor(180, 200, 170);
        private static RgbColor _commandsBlockBackground = new RgbColor(50, 50, 50);

        public static void PrintCommandsBlock(IEnumerable<IConsoleCommandWizard> availableWizards, IConsoleCommandWizard activeWizard)
        {
            var previousForeground = Console.ForegroundColor;
            var previousBackground = Console.BackgroundColor;

            Console.WriteLine(GetColorizedConsoleText(new string('-', Console.WindowWidth), _commandsBlockForeground, _commandsBlockBackground));
            Console.WriteLine(GetColorizedConsoleText(Strings.ValidCommands.PadRight(Console.WindowWidth), _commandsBlockForeground, _commandsBlockBackground));

            foreach (var wizard in availableWizards)
            {
                PrintCommand(wizard, activeWizard == wizard);
            }

            Console.WriteLine(GetColorizedConsoleText(new string('-', Console.WindowWidth), _commandsBlockForeground, _commandsBlockBackground));
        }

        private static void PrintCommand(IConsoleCommandWizard wizard, bool isActive)
        {
            string commandDisplayText = $"  {wizard.CommandKey} - {wizard.Description}";

            var foreGround = isActive ? _activeCommandForeground : wizard is PresentationWizard ? _presentationCommandForeground : _commandsBlockForeground;

            Console.WriteLine(GetColorizedConsoleText(commandDisplayText.PadRight(Console.WindowWidth), foreGround, _commandsBlockBackground));
        }
        public static void PrintSubCommandBlock(string commandKey, string subCommand)
        {
            string commandKeyRaw = $"| {commandKey} |";
            string commandKeyColored = GetColorizedConsoleText(commandKeyRaw, _activeCommandForeground, _commandsBlockBackground);

            string escRaw = Strings.EscapeToMainCommand;
            string escColored = GetColorizedConsoleText(escRaw, _escCommandForeground, _commandsBlockBackground);

            string subCommandRaw = $" - {subCommand}".PadRight(Console.WindowWidth - escRaw.Length - commandKeyRaw.Length);
            string subCommandColored = GetColorizedConsoleText(subCommandRaw, _commandsBlockForeground, _commandsBlockBackground);

            Console.Write($"{commandKeyColored}{subCommandColored}{escColored}");
            Console.WriteLine(GetColorizedConsoleText(new string('-', Console.WindowWidth), _commandsBlockForeground, _commandsBlockBackground));
        }

        public static void DisplayCards(IEnumerable<DevelopmentCard> cards)
        {
            foreach (var card in cards)
            {
                PrintCardWithCosts(card);
            }
        }

        public static void DisplayCards(IEnumerable<PurchaseAction> purchaseActions)
        {
            int key = 1;
            foreach (var purchase in purchaseActions)
            {
                PrintCardWithSpendingInfo(key++, purchase.Card, purchase.RequiredTokens);
            }
        }

        public static void DisplayLeaders(IEnumerable<Leader> leaders)
        {
            foreach (var leader in leaders)
            {
                PrintLeader(leader);
            }
        }

        public static void ClearFromLine(int startLine)
        {
            Console.SetCursorPosition(0, startLine);
            Console.Write("\x1b[J");
        }

        private static void PrintLeader(Leader leader)
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

            Console.WriteLine($"{leader.Id,-5} {leader.Name}{namePadding} Feltételek: {colorCost}");
        }

        private static void PrintCardWithCosts(DevelopmentCard card)
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

            Console.WriteLine($"{card.Id,-5} {coloredName}{namePadding}   {vpDisplay}   {colorCost}{costPadding}");
        }

        private static void PrintCardWithSpendingInfo(int key, DevelopmentCard card, TokenGroup requiredTokens)
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

            Console.WriteLine($"{key,-5} {coloredName}{namePadding}   {vpDisplay}   {completeCostText}");
        }

        private static string GetColorizedConsoleText(object text, RgbColor? foreground = null, RgbColor? background = null)
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
}