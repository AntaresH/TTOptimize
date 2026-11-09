using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.CLI;
using SplendidWonders.Common.Setup;
using System.Runtime.InteropServices;
using TTOptimize.Core;
using TTOptimize.Platform;

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
        // services.AddForestShuffle();

        var rootServiceProvider = services.BuildServiceProvider();

        var sessionManager = rootServiceProvider.GetRequiredService<GameSessionManager>();

        var gameConfiguration = new GameConfiguration()
        {
            PlayerNames = new List<string>() { "Ambrus", "Brünhilda" },
            UseExtendedLeaders = true,
            UseRandomSeed = true
        };

        sessionManager.StartNewMatch(Games.SPLENDID_WONDERS, gameConfiguration);
    }
}