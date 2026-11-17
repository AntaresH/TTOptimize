
using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.ViewAbstractions;

public record class BoardRenderData(IGameState State, IReadOnlyList<GameAction>? ValidActions);
public record class EventLogRenderData(IReadOnlyList<ActionLogEntry> History);
public record class CommandRenderData(List<string> RenderLines);
public record class ResultsRenderData(IGameState State, IGameRules Rules);
