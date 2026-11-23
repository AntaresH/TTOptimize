using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.CLI;
using System.Runtime.InteropServices;
using TTOptimize.ConsoleUI.Input;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.ViewAbstractions;
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

            string interactiveModeArg = settingsService.GetSetting(TTOptArguments.INTERACTIVE_MODE);
            bool interActiveMode = bool.TryParse(interactiveModeArg, out bool parsed) ? parsed : true;

            if (interActiveMode)
            {
                Console.Clear();
                DisplayTTOptimizeTitle();
                Console.WriteLine(Strings.PromptGameChooser);
            }

            var selectedGame = gameSelectionService.SelectGame();
            var sessionData = setupService.SetupGame(selectedGame);

            while (stayInGame)
            {
                try
                {
                    var sessionManager = rootProvider.GetRequiredService<GameSessionManager>();
                    sessionManager.StartNewMatch(selectedGame.GameId, sessionData.GameConfiguration, sessionData.Agents);
                }
                catch (ConsoleUserAbortException)
                {
                }

                stayInGame = interActiveMode ? StayPrompter(Strings.PromptStayInGame) : false;
            }

            stayInApp = interActiveMode ? StayPrompter(Strings.PromptStayInApp) : false;
        }
    }

    private static void RegisterServices(string[] args, ServiceCollection services)
    {
        services.AddTTOptimizeEngine();
        services.AddSplendidWonders();

        services.AddSingleton<SetupService>();
        services.AddSingleton<GameSelectionService>();
        services.AddSingleton<ISettingsService>((isp) => new SettingsService(args));
        services.AddSingleton<ConxelCompiler>();

        services.AddKeyedScoped<IGameView, ConxelView>(Games.SPLENDID_WONDERS_KEY);
        services.AddScoped<ICommandTerminal, ConxelTerminal>();
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