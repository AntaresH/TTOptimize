using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core;

namespace TTOptimize.ConsoleHost
{
    public class GameSelectionService
    {
        private readonly IServiceProvider _serviceProvider;

        public GameSelectionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IGameManifest SelectGame()
        {
            var settingsService = _serviceProvider.GetRequiredService<SettingsService>();
            var manifests = _serviceProvider.GetServices<IGameManifest>().ToList();

            for (int i = 0; i < manifests.Count; i++)
            {
                Console.WriteLine($"({i + 1}) - {manifests[i].DisplayName} - {manifests[i].Description}");
            }

            while (true)
            {
                string gameId = settingsService.QuickRead(TTOptArguments.GAMEID);

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
}
