using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Engine;

namespace TTOptimize.ConsoleHost;

public class ConsoleGameSetupService
{
    private readonly IServiceProvider _serviceProvider;

    public ConsoleGameSetupService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IGameConfiguration ReadConfiguration(Type configurationType)
    {
        if (!typeof(IGameConfiguration).IsAssignableFrom(configurationType))
        {
            throw new ArgumentException($"A megadott típus nem implementálja az IGameConfiguration interfészt: {configurationType.Name}");
        }

        var configInstance = (IGameConfiguration)Activator.CreateInstance(configurationType)!;

        Console.WriteLine("\n=== JÁTÉK BEÁLLÍTÁSOK ===");

        foreach (var property in configurationType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var attribute = property.GetCustomAttribute<GameConfigParamAttribute>();
            if (attribute == null) continue;

            // Jelenleg int és bool típusokra készítjük fel
            if (property.PropertyType == typeof(int))
            {
                int defaultValue = (int)(property.GetValue(configInstance) ?? attribute.Min);
                int value = PromptInt(attribute.DisplayName, attribute.Min, attribute.Max, defaultValue);
                property.SetValue(configInstance, value);
            }
            else if (property.PropertyType == typeof(bool))
            {
                bool defaultValue = (bool)(property.GetValue(configInstance) ?? false);
                bool value = PromptBool(attribute.DisplayName, defaultValue);
                property.SetValue(configInstance, value);
            }
        }

        return configInstance;
    }

    public IReadOnlyList<IPlayerAgent> CreateAgents(string gameId, int playerCount)
    {
        var agents = new List<IPlayerAgent>(playerCount);

        var gameInput = _serviceProvider.GetRequiredKeyedService<IGameInput>(gameId);

        Console.WriteLine("\n=== JÁTÉKOSOK BEÁLLÍTÁSA ===");

        for (int i = 0; i < playerCount; i++)
        {
            Console.WriteLine($"\n--- {i + 1}. Játékos ---");
            string name = PromptString($"Játékos neve (Enter = Játékos {i + 1}): ", $"Játékos {i + 1}");
            int agentType = PromptInt("Típus [1 = Ember, 2 = Random Bot]: ", 1, 2, 1);

            IPlayerAgent agent = agentType switch
            {
                1 => new HumanAgent(name),
                2 => new RandomAgent(name),
                _ => throw new InvalidOperationException("Ismeretlen ágens típus.")
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
}
