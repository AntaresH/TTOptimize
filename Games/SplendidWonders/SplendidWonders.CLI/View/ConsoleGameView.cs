using SplendidWonders.CLI.Input;
using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay.Actions;
using System.Collections.Immutable;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.View
{
    public class ConsoleGameView : ISplendidWondersView
    {
        private readonly ConsoleGameStateRenderer _gameStateRenderer = new();

        public void Render(IGameState gameState, IEnumerable<GameAction> validActions, string customMessage)
        {
            DisplayGame((GameState) gameState, validActions, customMessage);
        }

        public void Render(IGameState gameState, IEnumerable<GameAction> validActions)
        {
            Render(gameState, validActions, string.Empty);
        }

        public void RenderFinalState(IGameState gameState, object finalStandingsData)
        {
            DisplayBoard((GameState)gameState);
            DisplayGameOver((List<Player>) finalStandingsData);
        }

        public void DisplayGameOver(List<Player> players)
        {
            Console.WriteLine(Strings.GameOverTitle);
            Console.WriteLine();
            Console.WriteLine($"{Strings.Winner}: {players.First().Name}");
            Console.WriteLine();
            Console.WriteLine(Strings.FinalScores);
            Console.WriteLine();

            for (int i = 0; i < players.Count; i++)
            {
                Console.WriteLine($"#{i+1}: {players[i].Name} - {players[i].Tableau.Sum(c => c.VictoryPoints) + players[i].AcquiredLeaders.Count * 3} {Strings.VP}");
            }
        }

        public void DisplayGame(GameState gameState, IEnumerable<GameAction> validActions, string previousInvalidCommand)
        {
            DisplayBoard(gameState, validActions);

            if (!string.IsNullOrEmpty(previousInvalidCommand))
            {
                Console.WriteLine();
                PrintColoredLine(previousInvalidCommand, ConsoleColor.Red);
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        public void DisplayBoard(GameState gameState)
        {
            Console.Clear();
            DisplayGameTitle();
            _gameStateRenderer.Render(gameState, null);
        }

        public void DisplayBoard(GameState gameState, IEnumerable<GameAction> validActions)
        {
            Console.Clear();
            DisplayGameTitle();
            _gameStateRenderer.Render(gameState, validActions.OfType<PurchaseAction>());
        }

        public void DisplayValidActions(IEnumerable<GameAction> validActions)
        {
            Console.WriteLine(Strings.ValidCommands);

            foreach (var command in validActions)
            {
                string shorthand = GenericUtils.CommandShortHandMap.ContainsKey(command.GetType().Name) ? GenericUtils.CommandShortHandMap[command.GetType().Name] : string.Empty;
                Console.WriteLine($"{command.GetType().Name} ({shorthand})");
            }
        }

        public void DisplayInvalidCommandMessage()
        {
            Console.WriteLine(Strings.InvalidCommandState);
        }

        public void DisplayInvalidCommand(GameAction command)
        {
            Console.WriteLine(string.Format(Strings.InvalidCommandTypeStateFormat, command.GetType().Name));
        }

        public void DisplayGameTitle()
        {
            Console.WriteLine("   _____       _                _ _     _  __          __             _               ");
            Console.WriteLine("  / ____|     | |              | (_)   | | \\ \\        / /            | |              ");
            Console.WriteLine(" | (___  _ __ | | ___ _ __   __| |_  __| |  \\ \\  /\\  / /__  _ __   __| | ___ _ __ ___ ");
            Console.WriteLine("  \\___ \\| '_ \\| |/ _ \\ '_ \\ / _` | |/ _` |   \\ \\/  \\/ / _ \\| '_ \\ / _` |/ _ \\ '__/ __|");
            Console.WriteLine("  ____) | |_) | |  __/ | | | (_| | | (_| |    \\  /\\  / (_) | | | | (_| |  __/ |  \\__ \\");
            Console.WriteLine(" |_____/| .__/|_|\\___|_| |_|\\__,_|_|\\__,_|     \\/  \\/ \\___/|_| |_|\\__,_|\\___|_|  |___/");
            Console.WriteLine("        | |                                                                           ");
            Console.WriteLine("        |_|                                                                           ");
            Console.WriteLine("");
        }

        public static void PrintColoredLine(string text, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = previousColor;
        }
    }
}
