using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input.Wizards
{
    public abstract class SplendidWondersPresentationWizard : PresentationWizard
    {
        protected ISplendidWondersInputPresenter _inputPresenter;

        protected SplendidWondersPresentationWizard(
            IConsolePromptContext promptContext,
            ISplendidWondersInputPresenter inputPresenter
            ) : base(promptContext)
        {
            _inputPresenter = inputPresenter;
        }
    }
}