using HungerHydra.ViewModel;
using System.Numerics;

namespace HungerHydra.Views;

public partial class GamePage
{
    private readonly GameViewModel _viewModel;

    public GamePage()
    {
        InitializeComponent();

        BindingContext = _viewModel = new GameViewModel();

        HydraCanvas.PaintSurface += _viewModel.HydraCanvasPaintSurface;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        _viewModel.StartAnimationLoop(this, HydraCanvas);
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        _viewModel.SetPosition((float)width, (float)height * 10 / 11);
    }

    private void TapGestureRecognizer_Tapped(object? sender, TappedEventArgs e)
    {
        var relativeToContainerPosition = e.GetPosition((View?)sender);
        if (relativeToContainerPosition != null)
        {
            _viewModel.TapPoint = new Vector2((float)relativeToContainerPosition.Value.X,
                (float)relativeToContainerPosition.Value.Y);
        }
    }
}