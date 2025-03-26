using HungerHydra.Enums;
using HungerHydra.Helpers;
using HungerHydra.ViewModel;

namespace HungerHydra.Popups;

public partial class GameOverPopup
{
    private readonly GameOverViewModel _viewModel;

	public GameOverPopup(string score, string highScore)
	{
		InitializeComponent();
        BindingContext = _viewModel = new GameOverViewModel(score,highScore);
    }

    private async Task ExecuteButtonClickAsync(DialogReturnStatuses status)
    {
        if (IsBusy)
        {
            return;
        }
        IsBusy = true;

        await HideAsync();

        _viewModel.SetReturnValue(status);

        IsBusy = false;
    }

    public override async Task<DialogReturnValue?> ShowAsync(bool animate = true)
    {
        await base.ShowAsync(animate);

        return await _viewModel.ReturnValueAsync();
    }

    private void OnPositiveButtonClicked(object sender, EventArgs e)
    {
        _ = ExecuteButtonClickAsync(DialogReturnStatuses.Positive);
    }

    private void OnNegativeButtonClicked(object sender, EventArgs e)
    {
        _ = ExecuteButtonClickAsync(DialogReturnStatuses.Negative);
    }
}