using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using SplendidWonders.CLI;
using SplendidWonders.CLI.View;
using SplendidWonders.Common.Setup;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using TTOptimize.ConsoleHost.ConsoleView;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
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

        services.AddTTOptimizeEngine();
        services.AddSplendidWonders();

        services.AddSingleton<SetupService>();
        services.AddSingleton<GameSelectionService>();
        services.AddSingleton((isp) => new ArgumentService(args));
        services.AddSingleton<ConxelAnsiCompiler>();
        
        services.AddKeyedScoped<IGameView>(Games.SPLENDID_WONDERS_KEY, (sp, key) =>
        {
            var layoutManager = sp.GetRequiredKeyedService<ILayoutManager>(key);
            var compiler = sp.GetRequiredService<ConxelAnsiCompiler>();
            return new ConxelGameView(layoutManager, compiler);
        });

        var rootProvider = services.BuildServiceProvider();

        var gameSelectionService = rootProvider.GetRequiredService<GameSelectionService>();
        var setupService = rootProvider.GetRequiredService<SetupService>();
        var argumentService = rootProvider.GetRequiredService<ArgumentService>();

        bool stayInApp = true;

        while (stayInApp)
        {
            bool stayInGame = true;

            string interactiveModeArg = argumentService.GetArgument(TTOptArguments.INTERACTIVE_MODE);
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