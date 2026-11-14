using SplendidWonders.CLI.View;
using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using SplendidWonders.Gameplay.Actions;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input
{
    public static class ConsoleInputDisplayHelper
    {
        public static void DisplayActions(IEnumerable<IConsoleCommandWizard> activeWizards)
        {
            Console.WriteLine(Strings.ValidCommands);

            foreach (var wizard in activeWizards)
            {
                if (wizard is PresentationWizard)
                {
                    PrintColoredLine($"{wizard.CommandKey} - {wizard.Description}", ConsoleColor.Yellow);
                }
                else
                {
                    Console.WriteLine($"{wizard.CommandKey} - {wizard.Description}");
                }
            }

            Console.WriteLine();
        }

        public static void DisplayCards(IEnumerable<DevelopmentCard> cards)
        {
            foreach (var card in cards)
            {
                PrintCard(card);
            }
        }

        public static void DisplayCards(IEnumerable<PurchaseAction> purchaseActions)
        {
            foreach (var purchase in purchaseActions)
            {
                PrintCard(purchase.Card, purchase.RequiredPolymathsCount);
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
                string whiteText = GetWhiteConsoleForegroundText(leader.Requirements.Yellow.ToString());
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {whiteText} ", DevelopmentColor.Yellow));
            }

            if (leader.Requirements.Blue > 0)
            {
                string whiteText = GetWhiteConsoleForegroundText(leader.Requirements.Blue.ToString());
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {whiteText} ", DevelopmentColor.Blue));
            }

            if (leader.Requirements.Green > 0)
            {
                string whiteText = GetWhiteConsoleForegroundText(leader.Requirements.Green.ToString());
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {whiteText} ", DevelopmentColor.Green));
            }

            if (leader.Requirements.Brown > 0)
            {
                string whiteText = GetWhiteConsoleForegroundText(leader.Requirements.Brown.ToString());
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {whiteText} ", DevelopmentColor.Brown));
            }

            if (leader.Requirements.Red > 0)
            {
                string whiteText = GetWhiteConsoleForegroundText(leader.Requirements.Red.ToString());
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {whiteText} ", DevelopmentColor.Red));
            }

            string namePadding = new string(' ', Math.Max(0, 18 - leader.Name.Length));
            string colorCost = string.Join(" ", colorCostTexts);

            Console.WriteLine($"{leader.Id,-5} {leader.Name}{namePadding} Feltételek: {colorCost}");
        }

        private static void PrintCard(DevelopmentCard card, int requiredPolymathsCount = 0)
        {
            List<string> colorCostSubTexts = new List<string>();

            if (card.Cost.Yellow > 0)
            {
                colorCostSubTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Yellow.ToString(), DevelopmentColor.Yellow));
            }

            if (card.Cost.Blue > 0)
            {
                colorCostSubTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Blue.ToString(), DevelopmentColor.Blue));
            }

            if (card.Cost.Green > 0)
            {
                colorCostSubTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Green.ToString(), DevelopmentColor.Green));
            }

            if (card.Cost.Brown > 0)
            {
                colorCostSubTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Brown.ToString(), DevelopmentColor.Brown));
            }

            if (card.Cost.Red > 0)
            {
                colorCostSubTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Red.ToString(), DevelopmentColor.Red));
            }

            string vpDisplay = card.VictoryPoints > 0 ? $"{{{card.VictoryPoints}}}" : "   ";

            string namePadding = new string(' ', Math.Max(0, 26 - card.Name.Length));
            string coloredName = GetRgbColoredConsoleForeground(card.Name, card.Bonus);

            string colorCost = string.Join(" ", colorCostSubTexts);
            string costPadding = new string(' ', Math.Max(0, 12 - colorCostSubTexts.Count * 2 -1));
            string polymathsDisplay = requiredPolymathsCount > 0 ? $"Polihisztor: {requiredPolymathsCount}" : string.Empty;

            Console.WriteLine($"{card.Id,-5} {coloredName}{namePadding}   {vpDisplay}   Költségek: {colorCost}{costPadding} {polymathsDisplay}");
        }

        private static string GetWhiteConsoleForegroundText(string text)
        {
            return $"\u001b[38;2;255;255;255m{text}\u001b[39m";
        }

        private static string GetRgbColoredConsoleForeground(string text, DevelopmentColor trueColorRgbFormat)
        {
            return $"\u001b[38;2;{ConvertBonusToLightColorSet(trueColorRgbFormat)}m{text}\u001b[39m";
        }

        private static string GetRgbColoredConsoleBackground(string text, DevelopmentColor trueColorRgbFormat)
        {
            return $"\u001b[48;2;{ConvertBonusToDarkColorSet(trueColorRgbFormat)}m{text}\u001b[49m";
        }

        public static string ConvertBonusToLightColorSet(DevelopmentColor bonus)
        {
            return bonus switch
            {
                DevelopmentColor.Yellow => "208;208;0",
                DevelopmentColor.Blue => "80;145;255",
                DevelopmentColor.Green => "0;220;50",
                DevelopmentColor.Brown => "145;92;42",
                DevelopmentColor.Red => "255;80;80",
                _ => "192;192;192"
            };
        }

        private static string ConvertBonusToDarkColorSet(DevelopmentColor bonus)
        {
            return bonus switch
            {
                DevelopmentColor.Yellow => "138;116;0",
                DevelopmentColor.Blue => "30;60;165",
                DevelopmentColor.Green => "0;125;30",
                DevelopmentColor.Brown => "100;45;0",
                DevelopmentColor.Red => "180;10;20",
                _ => "96;96;96"
            };
        }

        private static void PrintColoredLine(string text, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = previousColor;
        }
    }
}