using TTOptimize.Core.GameAbstractions;

namespace TTOptimize.Core.InputAbstractions
{
    public abstract class ApplicationCommand
    {
        public string CommandText { get; set; }
    }

    public class GameCommand : ApplicationCommand
    {
        public GameAction Action { get; set; }
    }

    public class PresentationCommand : ApplicationCommand{ }
}
