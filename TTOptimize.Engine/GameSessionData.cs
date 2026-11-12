using TTOptimize.Core.Agents;
using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Engine
{
    public record class GameSessionData
    {
        public IGameConfiguration GameConfiguration { get; set; }
        public IReadOnlyList<IPlayerAgent> Agents { get; set; }
    }
}