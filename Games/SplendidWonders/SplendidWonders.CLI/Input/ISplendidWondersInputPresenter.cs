using SplendidWonders.Common.Actions;
using SplendidWonders.Common.Model;
using TTOptimize.Core.InputAbstractions;

namespace SplendidWonders.CLI.Input;

public interface ISplendidWondersInputPresenter
{
    void DisplayCommandBlock(IEnumerable<ITerminalWizard> wizards, ITerminalWizard activeWizard);
    void DisplaySubCommandBlock(string commandKey, string subCommand);
    void DisplayCards(IEnumerable<DevelopmentCard> cards);
    void DisplayPurchases(IEnumerable<PurchaseAction> purchaseActions);
    void DisplayLeaders(IEnumerable<Leader> leaders);
}
