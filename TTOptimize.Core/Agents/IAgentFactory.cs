using TTOptimize.Core.InputAbstractions;

namespace TTOptimize.Core.Agents;

public sealed record class AgentDescriptor(string AgentTypeId, string DisplayName, bool RequiresHumanInput = false);

public interface IAgentFactory
{
    IReadOnlyList<AgentDescriptor> SupportedAgents { get; }

    IAgent CreateAgent(string agentTypeId, string displayName, IGameInput? input = null);
}
