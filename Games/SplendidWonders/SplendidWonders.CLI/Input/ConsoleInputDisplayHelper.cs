using SplendidWonders.CLI.View;
using SplendidWonders.Common.Model;
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

        public static void DisplayLeaders(IEnumerable<Leader> leaders)
        {
            foreach (var leader in leaders)
            {
                PrintLeader(leader);
            }
        }
        public static void ClearFromLine(int startLine)
        {
            // 1. Visszaállítjuk a kurzort a mentett sor elejére
            Console.SetCursorPosition(0, startLine);

            // 2. ANSI Escape kód (\x1b[J): Töröl mindent a kurzortól a képernyő aljáig!
            Console.Write("\x1b[J");
        }

        private static void PrintLeader(Leader leader)
        {
            List<string> colorCostTexts = new List<string>();

            if (leader.Requirements.Yellow > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {leader.Requirements.Yellow.ToString()} ", DevelopmentBonus.Yellow));
            }

            if (leader.Requirements.Blue > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {leader.Requirements.Blue.ToString()} ", DevelopmentBonus.Blue));
            }

            if (leader.Requirements.Green > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {leader.Requirements.Green.ToString()} ", DevelopmentBonus.Green));
            }

            if (leader.Requirements.Brown > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {leader.Requirements.Brown.ToString()} ", DevelopmentBonus.Brown));
            }

            if (leader.Requirements.Red > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleBackground($" {leader.Requirements.Red.ToString()} ", DevelopmentBonus.Red));
            }

            string vpDisplay = leader.VictoryPoints > 0 ? $"{{{leader.VictoryPoints}}}" : string.Empty;

            Console.WriteLine($"{leader.Id} {leader.Name.PadRight(15)} {vpDisplay} Feltételek: {string.Join(" ", colorCostTexts)}");
        }

        private static void PrintCard(DevelopmentCard card)
        {
            List<string> colorCostTexts = new List<string>();

            if (card.Cost.Yellow > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Yellow.ToString(), DevelopmentBonus.Yellow));
            }

            if (card.Cost.Blue > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Blue.ToString(), DevelopmentBonus.Blue));
            }

            if (card.Cost.Green > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Green.ToString(), DevelopmentBonus.Green));
            }

            if (card.Cost.Brown > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Brown.ToString(), DevelopmentBonus.Brown));
            }

            if (card.Cost.Red > 0)
            {
                colorCostTexts.Add(GetRgbColoredConsoleForeground(card.Cost.Red.ToString(), DevelopmentBonus.Red));
            }

            string vpDisplay = card.VictoryPoints > 0 ? $"{{{card.VictoryPoints}}}" : "   ";

            string namePadding = new string(' ', Math.Max(0, 20 - card.Name.Length));
            string coloredName = GetRgbColoredConsoleForeground(card.Name, card.Bonus);

            Console.WriteLine($"{card.Id,-5} {coloredName}{namePadding} {vpDisplay} Költségek: {string.Join(" ", colorCostTexts)}");
        }

        private static string GetRgbColoredConsoleForeground(string text, DevelopmentBonus trueColorRgbFormat)
        {
            return $"\u001b[38;2;{ConvertRgbColorIntoText(trueColorRgbFormat)}m{text}\u001b[0m";
        }

        private static string GetRgbColoredConsoleBackground(string text, DevelopmentBonus trueColorRgbFormat)
        {
            return $"\u001b[48;2;{ConvertRgbColorIntoText(trueColorRgbFormat)}m{text}\u001b[0m";
        }

        private static string ConvertRgbColorIntoText(DevelopmentBonus bonus)
        {
            return bonus switch
            {
                DevelopmentBonus.Yellow => "208;208;0",
                DevelopmentBonus.Blue => "80;145;255",
                DevelopmentBonus.Green => "0;220;50",
                DevelopmentBonus.Brown => "145;92;42",
                DevelopmentBonus.Red => "255;80;80",
                _ => "192;192;192"
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