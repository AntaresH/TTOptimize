using SplendidWonders.Common.Model;
using SplendidWonders.Common.Setup;
using SplendidWonders.Gameplay.Actions;
using System;
using System.Collections.Generic;
using System.Text;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input
{
    public interface ISplendidWondersInputPresenter
    {
        void DisplayCommandBlock(IEnumerable<IConsoleCommandWizard> wizards, IConsoleCommandWizard activeWizard);
        void DisplaySubCommandBlock(string commandKey, string subCommand);
        void DisplayCards(IEnumerable<DevelopmentCard> cards);
        void DisplayPurchases(IEnumerable<PurchaseAction> purchaseActions);
        void DisplayLeaders(IEnumerable<Leader> leaders);
    }
}
