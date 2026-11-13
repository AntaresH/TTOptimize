using Microsoft.Extensions.DependencyInjection;
using SplendidWonders.CLI.View.Legacy;
using System.ComponentModel.Design;
using System.Reflection;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.ViewAbstractions;
using TTOptimize.Engine;

namespace TTOptimize.ConsoleHost;

public class SetupService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ArgumentService _argumentService;
    private readonly bool _isInterActive;

    public SetupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _argumentService = serviceProvider.GetRequiredService<ArgumentService>();

        var interActiveModeArg = _argumentService.GetArgument(TTOptArguments.INTERACTIVE_MODE);
        _isInterActive = bool.TryParse(interActiveModeArg, out bool parsed) ? parsed : true;
    }

    public GameSessionConfiguration SetupGame(IGameManifest selectedManifest)
    {   
        if (_isInterActive)
        {
            var titleView = _serviceProvider.GetRequiredKeyedService<ITitleView>(selectedManifest.GameId);

            Console.Clear();
            titleView.DisplayGameTitle();
        }

        IGameConfiguration config = ReadConfiguration(selectedManifest.ConfigurationType);
        IReadOnlyList<IPlayerAgent> agents = CreateAgents(selectedManifest.GameId, config.PlayerCount);

        config.PlayerNames = agents.Select(ag => ag.Name).ToList();

        return new GameSessionConfiguration()
        {
            GameConfiguration = config,
            Agents = agents
        };
    }

    public IGameConfiguration ReadConfiguration(Type configurationType)
    {
        if (!typeof(IGameConfiguration).IsAssignableFrom(configurationType))
        {
            throw new ArgumentException($"A megadott típus nem implementálja az IGameConfiguration interfészt: {configurationType.Name}");
        }

        var configInstance = (IGameConfiguration)Activator.CreateInstance(configurationType)!;

        if (_isInterActive)
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
                SetPropertyFromArgumentOrPrompt(property, configInstance, attribute.Min,
                    def => PromptInt(attribute.DisplayName, attribute.Min, attribute.Max, def));
            }
            else if (property.PropertyType == typeof(bool))
            {
                SetPropertyFromArgumentOrPrompt(property, configInstance, false,
                    def => PromptBool(attribute.DisplayName, def));
            }
        }

        return configInstance;
    }

    public IReadOnlyList<IPlayerAgent> CreateAgents(string gameId, int playerCount)
    {
        var agents = new List<IPlayerAgent>(playerCount);

        if (_isInterActive)
            Console.WriteLine($"\n=== {Strings.PlayerSettings} ===");

        for (int i = 0; i < playerCount; i++)
        {
            if (_isInterActive)
                Console.WriteLine($"\n--- {i + 1}. {Strings.Player} ---");

            string name = FromArgumentOrPrompt(string.Format(Strings.PlayerNameArgumentKeyFormat, i + 1), () => PromptString(string.Format(Strings.PlayerNamePrompt, i + 1), $"{Strings.Player} {i + 1}"));
            int agentType = FromArgumentOrPrompt(string.Format(Strings.AgentTypeArgumentKeyFormat, i + 1), () => PromptInt(Strings.AgentTypePrompt, 1, 2, 1));

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

    private void SetPropertyFromArgumentOrPrompt<T>(PropertyInfo property, object configInstance, 
        T fallbackDefault, Func<T, T> promptUser) where T : IParsable<T>
    {
        T defaultValue = (T)(property.GetValue(configInstance) ?? fallbackDefault!);

        string? propArg = _argumentService.GetArgument(property.Name);

        if (T.TryParse(propArg, null, out T parsedValue))
        {
            property.SetValue(configInstance, parsedValue);
        }
        else
        {
            T value = promptUser(defaultValue);
            property.SetValue(configInstance, value);
        }
    }

    private T FromArgumentOrPrompt<T>(string formattedArgumentKey, Func<T> promptUser) where T : IParsable<T>
    {
        string propertyArg = _argumentService.GetArgument(formattedArgumentKey);

        if (T.TryParse(propertyArg, null, out T parsedArg))
        {
            return parsedArg;
        }
        else
        {
            return promptUser();
        }
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

}
