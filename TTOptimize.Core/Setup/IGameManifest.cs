namespace TTOptimize.Core.Setup;

public interface IGameManifest
{
    string AsciiLogo { get; }

    string GameId { get; }

    string DisplayName { get; }

    string Description { get; }

    Type ConfigurationType { get; }
}
