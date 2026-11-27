using Microsoft.Extensions.DependencyInjection;
using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;
using TTOptimize.Core.Statistics;
using TTOptimize.Core.ViewAbstractions;

namespace TTOptimize.Engine;

public sealed record class GameContext(
    IGameStateFactory StateFactory, 
    IGameRules Rules, 
    IAgentFactory AgentFactory, 
    IMatchMetricsExtractor? MetricsExtractor,
    IGameView? View, 
    IGameInput? Input, 
    IServiceScope Scope) : IDisposable
{
    public void Dispose()
    {
        Scope.Dispose();
    }
}