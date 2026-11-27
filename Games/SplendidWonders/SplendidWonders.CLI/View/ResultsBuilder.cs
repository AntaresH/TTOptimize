using SplendidWonders.Common.Model;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public class ResultsBuilder : IConxelPaneBuilder<ResultsRenderData>
    {
        private int _height = LayoutConstants.RESULTSPANEHEIGHT;
        private int _width = LayoutConstants.BORDERWIDTH + LayoutConstants.PANEGAP + LayoutConstants.EVENTSWIDTH;

        public ConxelCanvas Build(ResultsRenderData data)
        {
            var conxelCanvas = new ConxelCanvas(_height, _width);

            var swState = (GameState)data.State;
            var playerResults = data.Results;

            conxelCanvas.WriteText(0, 0, $"{Strings.GameOverTitle} - {Strings.Winner}: {playerResults.First().Player.Name}");
            conxelCanvas.WriteText(2, 0, Strings.FinalScores);

            for (int i = 0; i < playerResults.Count; i++)
            {
                var playerResult = playerResults[i];
                string simpleResultString = $"#{playerResult.Rank}: {playerResult.Player.Name} - {playerResult.Points} {Strings.VP}";


                var formattedResultString =
                    playerResult.Tiebreaker.HasValue
                    ? $"{simpleResultString} - {playerResult.Tiebreaker.Value} {Strings.TiebreakerDevelopment}"
                    : simpleResultString;

                conxelCanvas.WriteText(i + 3, 0, formattedResultString);
            }

            return conxelCanvas;
        }
    }
}