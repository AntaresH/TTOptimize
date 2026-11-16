using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core;
using TTOptimize.Core.GameAbstractions;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input.Wizards
{
    public abstract class SplendidWondersBaseWizard : BaseConsoleCommandWizard
    {
        protected ISplendidWondersInputPresenter _inputPresenter;

        protected SplendidWondersBaseWizard(IConsolePromptContext promptContext, ISplendidWondersInputPresenter inputPresenter) : base(promptContext)
        {
            _inputPresenter = inputPresenter;
        }
    }
}
