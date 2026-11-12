using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using SplendidWonders.CLI;
using SplendidWonders.CLI.View;
using SplendidWonders.Common.Setup;
using System.Runtime.InteropServices;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Engine;

namespace TTOptimize.ConsoleHost;

public class Program
{
    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private const int SW_MAXIMIZE = 3;

    public static void Main(string[] args)
    {
        IntPtr handle = GetConsoleWindow();

        if (handle != IntPtr.Zero)
        {
            ShowWindow(handle, SW_MAXIMIZE);
        }

        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.InputEncoding = System.Text.Encoding.UTF8;

        var services = new ServiceCollection();

        services.AddTTOptimizeEngine();
        services.AddSplendidWonders();

        services.AddSingleton<ConsoleGameSetupService>();

        var rootProvider = services.BuildServiceProvider();

        var manifests = rootProvider.GetServices<IGameManifest>().ToList();
        var setupService = rootProvider.GetRequiredService<ConsoleGameSetupService>();
        var sessionManager = rootProvider.GetRequiredService<GameSessionManager>();

        bool stayInApp = true;

        while (stayInApp)
        {
            bool stayInGame = true;

            while (stayInGame)
            {
                Console.Clear();
                DisplayTTOptimizeTitle();
                Console.WriteLine(Strings.PromptGameChooser);

                for (int i = 0; i < manifests.Count; i++)
                {
                    Console.WriteLine($"({i + 1}) - {manifests[i].DisplayName} - {manifests[i].Description}");
                }

                var input = Console.ReadLine().Trim();

                if (int.TryParse(input, out int parsedNum) && parsedNum > 0 && parsedNum <= manifests.Count)
                {
                    var gameView = rootProvider.GetRequiredKeyedService<ISplendidWondersView>(manifests[parsedNum-1].GameId);

                    Console.Clear();
                    gameView.DisplayGameTitle();

                    try
                    {
                        var selectedManifest = manifests[parsedNum - 1];

                        IGameConfiguration config = setupService.ReadConfiguration(selectedManifest.ConfigurationType);
                        IReadOnlyList<IPlayerAgent> agents = setupService.CreateAgents(selectedManifest.GameId, config.PlayerCount);

                        config.PlayerNames = agents.Select(ag => ag.Name).ToList();

                        sessionManager.StartNewMatch(selectedManifest.GameId, config, agents);
                    }
                    catch (ConsoleUserAbortException)
                    {
                    }

                    stayInGame = StayPrompter(Strings.PromptStayInGame);
                }
            }

           stayInApp = StayPrompter(Strings.PromptStayInApp);
        }
    }

    private static void DisplayTTOptimizeTitle()
    {
        Console.WriteLine(" _______  _______  _______  ______  _______  __   __   __  __   ______  _______ \r\n" +
                            "|__   __||__   __||   _   ||   _  ||__  __| |  | |  |_|  ||  | |____  ||    ___|\r\n" +
                            "   | |      | |   |  | |  ||  |_| |   | |   |  | |       ||  |  ____| ||   |___ \r\n" +
                            "   | |      | |   |  | |  ||   ___|   | |   |  | | || || ||  | | _____||    ___|\r\n" +
                            "   | |      | |   |  |_|  ||  |       | |   |  | | ||_|| ||  | | |____ |   |___ \r\n" +
                            "   |_|      |_|   |_______||__|       |_|   |__| |_|   |_||__| |______||_______|");
        Console.WriteLine();
        Console.WriteLine();
    }

    private static bool StayPrompter(string message)
    {
        while (true)
        {
            Console.WriteLine(message);

            var userInput = Console.ReadLine().Trim();

            if (userInput == Strings.YesShort || userInput == Strings.Yes)
            {
                return true;
            }
            else if (userInput == Strings.NoShort || userInput == Strings.No)
            {
                return false;
            }
        }
    }
}