using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using SplendidWonders.CLI;
using SplendidWonders.Common.Setup;
using System.Runtime.InteropServices;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
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

        // 1. Lekérjük az elérhető játékokat és a segédosztályokat
        var manifests = rootProvider.GetServices<IGameManifest>().ToList();
        var setupService = rootProvider.GetRequiredService<ConsoleGameSetupService>();
        var sessionManager = rootProvider.GetRequiredService<GameSessionManager>();

        bool stayInApp = true;

        while (stayInApp)
        {
            var selectedManifest = manifests.First();

            IGameConfiguration config = setupService.ReadConfiguration(selectedManifest.ConfigurationType);
            IReadOnlyList<IPlayerAgent> agents = setupService.CreateAgents(selectedManifest.GameId, config.PlayerCount);

            sessionManager.StartNewMatch(selectedManifest.GameId, config, agents);

            StayPrompter();
        }
    }

    private static bool StayPrompter()
    {
        while (true)
        {
            Console.WriteLine(Strings.PlayAgain);

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