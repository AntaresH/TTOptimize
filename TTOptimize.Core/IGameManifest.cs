namespace TTOptimize.Core;

public interface IGameManifest
{
    string GameId { get; }

    string DisplayName { get; }

    string Description { get; }

    Type ConfigurationType { get; }
}
