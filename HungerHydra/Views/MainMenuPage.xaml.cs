using HungerHydra.ViewModel;

namespace HungerHydra.Views;

public partial class MainMenuPage
{
	public MainMenuPage()
	{
		InitializeComponent();

        BindingContext = new NavigationViewModel();
    }
}