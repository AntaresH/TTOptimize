using SplendidWonders.CLI.View.Legacy;
using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.ViewAbstractions;
using static System.Net.Mime.MediaTypeNames;

namespace SplendidWonders.CLI.View
{
    public class SplendidWondersView : IGameView
    {
        public void DisplayGameOver(IGameState state, IGameRules rules)
        {
            var swState = (GameState) state;

            var players = (List<Player>)rules.GetFinalStandings(state);

            Console.WriteLine($"{Strings.GameOverTitle} - {Strings.Winner}: {players.First().Name}");
            Console.WriteLine();
            Console.WriteLine(Strings.FinalScores);

            for (int i = 0; i < players.Count; i++)
            {
                Console.WriteLine($"#{i + 1}: {players[i].Name} - {players[i].Tableau.Sum(c => c.VictoryPoints) + players[i].AcquiredLeaders.Count * 3} {Strings.VP}");
            }
            Console.WriteLine();
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
        }

        public void DisplayValidationFailure(string lastValidationFailure)
        {
            Console.WriteLine();
            PrintColoredLine(lastValidationFailure, ConsoleColor.Red);
        }

        private void PrintColoredLine(string text, ConsoleColor color)
        {
            var previousColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ForegroundColor = previousColor;
        }
    }
}
