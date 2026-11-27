using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents;

public class BaseAgentFactory : IAgentFactory
{
    public const string HUMAN_AGENT_ID = "human";
    public const string RANDOM_AGENT_ID = "random";

    private static readonly IReadOnlyList<AgentDescriptor> _builtInAgents =
    [
        new AgentDescriptor(HUMAN_AGENT_ID, "Human Player", RequiresHumanInput: true),
        new AgentDescriptor(RANDOM_AGENT_ID, "Random Agent", RequiresHumanInput: false)
    ];

    protected virtual IEnumerable<AgentDescriptor> GameSpecificAgents => [];

    public IReadOnlyList<AgentDescriptor> SupportedAgents => _builtInAgents.Concat(GameSpecificAgents).ToList();

    public IAgent CreateAgent(string agentTypeId, string displayName, IGameInput? input = null)
    {
        if (string.Equals(agentTypeId, HUMAN_AGENT_ID, StringComparison.OrdinalIgnoreCase))
        {
            if (input is null)
            {
                throw new InvalidOperationException($"Cannot create human agent '{displayName}' when IsGameInteractive is false.");
            }

            return new HumanAgent(displayName, input);
        }

        if (string.Equals(agentTypeId, RANDOM_AGENT_ID, StringComparison.OrdinalIgnoreCase))
        {
            return new RandomAgent(displayName);
        }

        return CreateGameSpecificAgent(agentTypeId, displayName, input) ?? throw new ArgumentException($"Unsupported agent type '{agentTypeId}'.", nameof(agentTypeId));
    }

    protected virtual IAgent? CreateGameSpecificAgent(string agentTypeId, string displayName, IGameInput? input) => null;
}
