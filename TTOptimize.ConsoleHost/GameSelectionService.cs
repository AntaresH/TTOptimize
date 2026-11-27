using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core;
using TTOptimize.Core.Setup;

namespace TTOptimize.ConsoleHost;

public class GameSelectionService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ISettingsService _settingsService;
    private readonly IReadOnlyList<IGameManifest> _gameManifests;

    public GameSelectionService(IServiceProvider serviceProvider, ISettingsService settingsService, IEnumerable<IGameManifest> gameManifests)
    {
        _serviceProvider = serviceProvider;
        _settingsService = settingsService;
        _gameManifests = gameManifests.ToList();
    }

    public IGameManifest SelectGame()
    {
        if (_settingsService.IsSetupInteractive)
        {
            for (int i = 0; i < _gameManifests.Count; i++)
            {
                Console.WriteLine($"({i + 1}) - {_gameManifests[i].DisplayName} - {_gameManifests[i].Description}");
            }
        }

        while (true)
        {
            object gameArg = _settingsService[TTOptArguments.GAMEID];
            string gameId = gameArg != null ? gameArg.ToString() : Console.ReadLine().Trim();

            if (int.TryParse(gameId, out int parsedNum) && parsedNum > 0 && parsedNum <= _gameManifests.Count)
            {
                return _gameManifests[parsedNum - 1];
            }

            var candidateManifest = _gameManifests.FirstOrDefault(man => man.GameId == gameId);

            if (candidateManifest != null)
                return candidateManifest;
        }
    }
}
