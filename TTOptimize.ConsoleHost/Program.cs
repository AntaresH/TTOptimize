using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.CLI;
using System.Runtime.InteropServices;
using TTOptimize.Core;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.Setup;
using TTOptimize.Engine;
using TTOptimize.Persistence;

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

        RegisterServices(args, services);

        var rootProvider = services.BuildServiceProvider();

        RunApp(rootProvider);
    }

    private static void RunApp(ServiceProvider rootProvider)
    {
        var gameSelectionService = rootProvider.GetRequiredService<GameSelectionService>();
        var setupService = rootProvider.GetRequiredService<SetupService>();
        var settingsService = rootProvider.GetRequiredService<ISettingsService>();

        bool stayInApp = true;

        while (stayInApp)
        {
            bool stayInGame = true;

            if (settingsService.IsInteractive)
            {
                Console.Clear();
                DisplayTTOptimizeTitle();
                Console.WriteLine(Strings.PromptGameChooser);
            }

            var selectedGame = gameSelectionService.SelectGame();
            var sessionData = setupService.SetupGame(selectedGame);

            int.TryParse(settingsService.GetSetting(TTOptArguments.BATCHRUNS), out int batchRuns);

            while (stayInGame)
            {
                try
                {
                    var sessionManager = rootProvider.GetRequiredService<GameSessionManager>();

                    if (batchRuns > 1)
                    {
                        var batchExecutionResult = sessionManager.RunBatch(selectedGame.GameId, sessionData.GameConfiguration, sessionData.Slots, batchRuns);

                        Console.WriteLine($"{Strings.BatchExecutionTime}: {Math.Round(batchExecutionResult.TotalDuration.TotalSeconds, 4)} {Strings.Seconds}");
                    }
                    else
                    {
                        var sessionResult = sessionManager.RunSingleMatch(selectedGame.GameId, sessionData.GameConfiguration, sessionData.Slots);

                        Console.WriteLine($"{Strings.ExecutionTime}: {Math.Round(sessionResult.ExecutionInfo.Elapsed.TotalSeconds, 4)} {Strings.Seconds}");
                    }
                }
                catch (ConsoleUserAbortException)
                {
                }
                finally
                {
                }

                batchRuns--;

                stayInGame = settingsService.IsInteractive ? StayPrompter(Strings.PromptStayInGame) : false;
            }

            stayInApp = settingsService.IsInteractive ? StayPrompter(Strings.PromptStayInApp) : false;
        }
    }

    private static void RegisterServices(string[] args, ServiceCollection services)
    {
        services.AddSingleton<ISettingsService>(_ => new SettingsService(args));
        services.AddSingleton<SetupService>();
        services.AddSingleton<GameSelectionService>();

        services.AddTTOptimizeEngine();
        services.AddTTOptimizeConsoleUI();

        string dbPath = Path.Combine(AppContext.BaseDirectory, "ttoptimize.db");
        services.AddTTOptimizePersistence(dbPath);

        services.AddSplendidWonders();
        services.AddConxelViewForGame(Games.SPLENDID_WONDERS_KEY);

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

            if (userInput == Strings.YesShort || userInput == Strings.YesLong)
            {
                return true;
            }
            else if (userInput == Strings.NoShort || userInput == Strings.NoLong)
            {
                return false;
            }
        }
    }
}