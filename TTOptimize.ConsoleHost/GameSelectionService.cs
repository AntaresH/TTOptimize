using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core;
using TTOptimize.Core.Setup;

namespace TTOptimize.ConsoleHost;

public class GameSelectionService
{
    private readonly IServiceProvider _serviceProvider;

    public GameSelectionService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IGameManifest SelectGame()
    {
        var settingsService = _serviceProvider.GetRequiredService<ISettingsService>();
        var manifests = _serviceProvider.GetServices<IGameManifest>().ToList();

        string interactiveModeArg = settingsService.GetSetting(TTOptArguments.INTERACTIVE_MODE);
        bool interActiveMode = bool.TryParse(interactiveModeArg, out bool parsed) ? parsed : true;

        if (interActiveMode)
        {
            for (int i = 0; i < manifests.Count; i++)
            {
                Console.WriteLine($"({i + 1}) - {manifests[i].DisplayName} - {manifests[i].Description}");
            }
        }

        while (true)
        {
            object gameArg = settingsService[TTOptArguments.GAMEID];
            string gameId = gameArg != null ? gameArg.ToString() : Console.ReadLine().Trim();

            if (int.TryParse(gameId, out int parsedNum) && parsedNum > 0 && parsedNum <= manifests.Count)
            {
                return manifests[parsedNum - 1];
            }

            var candidateManifest = manifests.FirstOrDefault(man => man.GameId == gameId);

            if (candidateManifest != null)
                return candidateManifest;
        }
    }
}
