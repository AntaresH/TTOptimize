using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TTOptimize.Core;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.Setup;
using TTOptimize.Engine;

namespace TTOptimize.ConsoleHost;

public class SetupService
{
    private bool _isInteractive;
    private readonly ISettingsService _settingsService;
    private static readonly string[] _botAgentNames = ["Metalhead", "RoboCube", "WireBeast", "B.O.R.G", "Skynet", "Omnius", "KaylonPrimary"];
    private readonly IEnumerator<string> _nameEnumerator = GetShuffledBotNames().GetEnumerator();

    public SetupService(ISettingsService settingsService)
    {
        _settingsService = settingsService;

        _isInteractive = _settingsService.IsSetupInteractive;
    }

    public GameSessionConfiguration SetupGame(IGameManifest selectedManifest, IAgentFactory agentFactory)
    {   
        if (_isInteractive)
        {
            Console.Clear();
            Console.Write(selectedManifest.AsciiLogo);
        }

        IGameConfiguration config = ReadConfiguration(selectedManifest.ConfigurationType);
        IReadOnlyList<PlayerSlotConfig> slots = CreatePlayerSlotConfigs(selectedManifest.GameId, agentFactory, config.PlayerCount);

        config.PlayerNames = slots.Select(ag => ag.DisplayName).ToList();

        return new GameSessionConfiguration()
        {
            GameConfiguration = config,
            Slots = slots
        };
    }

    public IGameConfiguration ReadConfiguration(Type configurationType)
    {
        if (!typeof(IGameConfiguration).IsAssignableFrom(configurationType))
        {
            throw new ArgumentException($"IGameConfiguration intrface is not implemented: {configurationType.Name}");
        }

        var configInstance = (IGameConfiguration)Activator.CreateInstance(configurationType)!;

        if (_isInteractive)
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
                SetPropertyFromSettingOrPrompt(property, configInstance, attribute.Min,
                    def => PromptInt(attribute.DisplayName, attribute.Min, attribute.Max, def));
            }
            else if (property.PropertyType == typeof(bool))
            {
                SetPropertyFromSettingOrPrompt(property, configInstance, false,
                    def => PromptBool(attribute.DisplayName, def));
            }
        }

        return configInstance;
    }

    public IReadOnlyList<PlayerSlotConfig> CreatePlayerSlotConfigs(string gameId, IAgentFactory agentFactory, int playerCount)
    {
        var slots = new List<PlayerSlotConfig>(playerCount);

        if (_isInteractive)
            Console.WriteLine($"\n=== {Strings.PlayerSettings} ===");

        var availableAgents = agentFactory.SupportedAgents;

        for (int i = 0; i < playerCount; i++)
        {
            if (_isInteractive)
                Console.WriteLine($"\n--- {i + 1}. {Strings.Player} ---");
            
            var agentTypeDictionary = new Dictionary<int, string>();
            List<string> agentTypePromptStrings = new List<string>();

            for (int j = 0; j < availableAgents.Count; j++)
            {
                var availableAgentTypeId = availableAgents[j].AgentTypeId;
                agentTypeDictionary.Add(j + 1, availableAgentTypeId);
                agentTypePromptStrings.Add($"{j + 1} - {availableAgentTypeId}");
            }

            string agentTypePrompt = $"[{string.Join(", ", agentTypePromptStrings)}])";
            int agentTypeKey = FromSettingOrPrompt(string.Format(Strings.AgentTypeArgumentKeyFormat, i + 1), () => PromptInt(agentTypePrompt, 1, agentTypeDictionary.Count, 1));
            
            if (!agentTypeDictionary.TryGetValue(agentTypeKey, out string? agentTypeId))
            {
                throw new InvalidOperationException($"Invalid agent type key '{agentTypeKey}' for Player {i + 1}.");
            }

            var isInteractiveAgent = availableAgents.FirstOrDefault(aa => aa.AgentTypeId == agentTypeId).RequiresHumanInput;

            string defaultPlayerName = isInteractiveAgent
                ? $"{Strings.Human} {Strings.Player} {i + 1}"
                : GenerateComputerAgentName();

            string name = FromSettingOrPrompt(string.Format(Strings.PlayerNameArgumentKeyFormat, i + 1), () => PromptString(string.Format(Strings.PlayerNamePrompt, defaultPlayerName), defaultPlayerName));

            var slot = new PlayerSlotConfig(i + 1, name, agentTypeId);

            slots.Add(slot);
        }

        return slots;
    }

    private void SetPropertyFromSettingOrPrompt<T>(PropertyInfo property, object configInstance, 
        T fallbackDefault, Func<T, T> promptUser) where T : IParsable<T>
    {
        T defaultValue = (T)(property.GetValue(configInstance) ?? fallbackDefault!);

        string? propArg = _settingsService[property.Name];

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

    private T FromSettingOrPrompt<T>(string formattedSettingKey, Func<T> promptUser) where T : IParsable<T>
    {
        string propertyArg = _settingsService[formattedSettingKey];

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

    private static IEnumerable<string> GetShuffledBotNames()
    {
        var pool = _botAgentNames.ToArray();

        while (true)
        {
            Random.Shared.Shuffle(pool);

            foreach (string name in pool)
            {
                yield return name;
            }
        }
    }

    private string GenerateComputerAgentName()
    {
        _nameEnumerator.MoveNext();
        return _nameEnumerator.Current;
    }
}
