using SplendidWonders.Common.Model;
using SplendidWonders.Gameplay;
using TTOptimize.ConsoleUI.View;
using TTOptimize.Core.ViewAbstractions;

namespace SplendidWonders.CLI.View
{
    public class SplendidWondersResultsBuilder : IConxelPaneBuilder<ResultsRenderData>
    {
        private int _columns = 149;
        private int _rows = 12;

        public ConxelCanvas Build(ResultsRenderData data)
        {
            var conxelCanvas = new ConxelCanvas(_rows, _columns);

            var swState = (GameState)data.State;
            var swPlayers = (List<Player>)data.FinalStandingsData;

            conxelCanvas.WriteText(0, 0, $"{Strings.GameOverTitle} - {Strings.Winner}: {swPlayers.First().Name}");
            conxelCanvas.WriteText(2, 0, Strings.FinalScores);

            for (int i = 3; i < swPlayers.Count; i++)
            {
                conxelCanvas.WriteText(i, 0, $"#{i + 1}: {swPlayers[i].Name} - {swPlayers[i].Tableau.Sum(c => c.VictoryPoints) + swPlayers[i].AcquiredLeaders.Count * 3} {Strings.VP}");
            }

            return conxelCanvas;
        }
    }
}