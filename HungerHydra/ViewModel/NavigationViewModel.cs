using HungerHydra.Abstractions;
using HungerHydra.Helpers;
using HungerHydra.Models;
using HungerHydra.Views;

namespace HungerHydra.ViewModel;

internal class NavigationViewModel : BaseNavigationViewModel
{
    public NavigationViewModel()
    {
        Items = new List<NavigationButtonItem>
        {
            new(Constants.Texts.StartGame, nameof(GamePage)),
            new(Constants.Texts.ScoresTitle, nameof(ScorePage))
        };
    }
}