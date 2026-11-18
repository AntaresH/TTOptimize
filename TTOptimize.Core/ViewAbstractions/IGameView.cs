namespace TTOptimize.Core.ViewAbstractions;

public interface IGameView
{
    void InitializeScreen();

    void RefreshBoard(BoardRenderData boardRenderData);
    void RefreshEvents(EventLogRenderData eventLogRenderData);
    void ShowResults(ResultsRenderData resultsRenderData);

    void DisplayValidationFailure(string lastValidationFailure);
}
