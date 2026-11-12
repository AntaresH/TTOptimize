using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.CLI.View;
using System.ComponentModel.Design;
using System.Reflection;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Engine;

namespace TTOptimize.ConsoleHost;

public class SetupService
{
    private readonly IServiceProvider _serviceProvider;

    public SetupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public GameSessionData SetupGame(IGameManifest selectedManifest)
    {
        var sessionManager = _serviceProvider.GetRequiredService<GameSessionManager>();
        
        var gameView = _serviceProvider.GetRequiredKeyedService<ISplendidWondersView>(selectedManifest.GameId);

        Console.Clear();
        gameView.DisplayGameTitle();

        IGameConfiguration config = ReadConfiguration(selectedManifest.ConfigurationType);
        IReadOnlyList<IPlayerAgent> agents = CreateAgents(selectedManifest.GameId, config.PlayerCount);

        config.PlayerNames = agents.Select(ag => ag.Name).ToList();

        return new GameSessionData()
        {
            GameConfiguration = config,
            Agents = agents
        };
    }

    public IGameConfiguration ReadConfiguration(Type configurationType)
    {
        var argumentService = _serviceProvider.GetRequiredService<ArgumentService>();

        if (!typeof(IGameConfiguration).IsAssignableFrom(configurationType))
        {
            throw new ArgumentException($"A megadott típus nem implementálja az IGameConfiguration interfészt: {configurationType.Name}");
        }

        var configInstance = (IGameConfiguration)Activator.CreateInstance(configurationType)!;

        Console.WriteLine($"\n=== {Strings.GameSettings} ===");

        foreach (var property in configurationType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var attribute = property.GetCustomAttribute<GameConfigParamAttribute>();
            if (attribute == null) continue;

            var dependsOn = property.GetCustomAttribute<DependsOnAttribute>();
            if (dependsOn != null)
            {
                var targetProp = configurationType.GetProperty(dependsOn.PropertyName);
                var actualValue = targetProp?.GetValue(configInstance);

                if (!Equals(actualValue, dependsOn.ExpectedValue)) 
                    continue;
            }

            if (property.PropertyType == typeof(int))
            {
                SetPropertyValue<int>(
                    property,
                    configInstance,
                    attribute.Min,
                    int.TryParse,
                    def => PromptInt(attribute.DisplayName, attribute.Min, attribute.Max, def)
                );
            }
            else if (property.PropertyType == typeof(bool))
            {
                SetPropertyValue<bool>(
                    property,
                    configInstance,
                    false,
                    bool.TryParse,
                    def => PromptBool(attribute.DisplayName, def)
                );
            }
        }

        return configInstance;
    }

    public IReadOnlyList<IPlayerAgent> CreateAgents(string gameId, int playerCount)
    {
        var agents = new List<IPlayerAgent>(playerCount);

        var gameInput = _serviceProvider.GetRequiredKeyedService<IGameInput>(gameId);
        var argumentService = _serviceProvider.GetRequiredService<ArgumentService>();

        Console.WriteLine($"\n=== {Strings.PlayerSettings} ===");

        for (int i = 0; i < playerCount; i++)
        {
            Console.WriteLine($"\n--- {i + 1}. {Strings.Player} ---");

            string name = string.Empty;
            string nameArg = argumentService.GetArgument($"PLAYER{i+1}NAME");
            if (!string.IsNullOrEmpty(nameArg))
            {
                name = nameArg;
            }
            else
            {
                name = PromptString($"{Strings.Player} {Strings.NameToEnter} (Enter = {Strings.Player} {i + 1}): ", $"{Strings.Player} {i + 1}");
            }

            int agentType = 0;
            string agentTypeArgString = argumentService.GetArgument($"PLAYER{i+1}TYPE");
            if (int.TryParse(agentTypeArgString, out int agentTypeArgInt))
            { 
                agentType = agentTypeArgInt;
            }
            else 
            {
                agentType = PromptInt($"{Strings.AgentType} [1 = {Strings.Human}, 2 = Random Bot]: ", 1, 2, 1);
            }

            IPlayerAgent agent = agentType switch
            {
                1 => new HumanAgent(name),
                2 => new RandomAgent(name),
                _ => throw new InvalidOperationException("Unknown agent type.")
            };

            agents.Add(agent);
        }

        return agents;
    }

    private int PromptInt(string label, int min, int max, int defaultValue)
    {
        while (true)
        {
            Console.Write($"{label} ({min}-{max}) [Alapértelmezett: {defaultValue}]: ");
            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input)) return defaultValue;

            if (int.TryParse(input, out int result) && result >= min && result <= max)
            {
                return result;
            }

            Console.WriteLine($"[Hiba] Kérlek {min} és {max} közötti számot adj meg!");
        }
    }

    private bool PromptBool(string label, bool defaultValue)
    {
        string defText = defaultValue ? "I" : "N";
        while (true)
        {
            Console.Write($"{label} (I/N) [Alapértelmezett: {defText}]: ");
            string? input = Console.ReadLine()?.Trim().ToUpper();

            if (string.IsNullOrEmpty(input)) return defaultValue;
            if (input is "I" or "IGEN" or "Y" or "YES") return true;
            if (input is "N" or "NEM" or "NO") return false;

            Console.WriteLine("[Hiba] Kérlek 'I' vagy 'N' betűt adj meg!");
        }
    }

    private string PromptString(string label, string defaultValue)
    {
        Console.Write(label);
        string? input = Console.ReadLine()?.Trim();
        return string.IsNullOrEmpty(input) ? defaultValue : input;
    }

    private delegate bool TryParseHandler<T>(string? value, out T result);

    private void SetPropertyValue<T>(
    PropertyInfo property,
    object configInstance,
    T fallbackDefault,
    TryParseHandler<T> tryParse,
    Func<T, T> promptUser)
    {
        var argumentService = _serviceProvider.GetRequiredService<ArgumentService>();

        T defaultValue = (T)(property.GetValue(configInstance) ?? fallbackDefault!);

        string? propArg = argumentService.GetArgument(property.Name);

        if (tryParse(propArg, out T parsedValue))
        {
            property.SetValue(configInstance, parsedValue);
        }
        else
        {
            T value = promptUser(defaultValue);
            property.SetValue(configInstance, value);
        }
    }
}
