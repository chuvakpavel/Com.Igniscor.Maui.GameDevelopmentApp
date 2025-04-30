using HungerHydra.ViewModel;

namespace HungerHydra.Views;

public partial class ScorePage
{
	public ScorePage()
	{
		InitializeComponent();

        BindingContext = new ScoreViewModel();
    }
}