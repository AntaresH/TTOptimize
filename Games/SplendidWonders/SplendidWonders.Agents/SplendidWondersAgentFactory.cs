using TTOptimize.Core.Agents;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.Agents;

public sealed class SplendidWondersAgentFactory : BaseAgentFactory
{
    public const string HEURISTIC_V1_ID = "heuristic_v1";

    protected override IEnumerable<AgentDescriptor> GameSpecificAgents =>
    [
        new AgentDescriptor(HEURISTIC_V1_ID, "Weighted Heuristic Bot (V1)")
    ];

    protected override IAgent? CreateGameSpecificAgent(string agentTypeId,string displayName, IGameInput? input)
    {
        return agentTypeId.ToLowerInvariant() switch
        {
            HEURISTIC_V1_ID => new HeuristicAgent(displayName),
            _ => null
        };
    }
}
