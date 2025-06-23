using HungerHydra.ViewModel;
using System.Numerics;
using HungerHydra.Enums;
using HungerHydra.Popups;

namespace HungerHydra.Views;

public partial class GamePage
{
    private bool _isPause;

    public bool IsPause
    {
        get => _isPause;
        set
        {
            if (value == false)
            {
                _viewModel.StartAnimationLoop(this, HydraCanvas, SpiderCanvas, DeadSpiderCanvas);
                _viewModel.StartLogicLoop(this);
                _viewModel.StartSpawnLoop(this);
            }

            _isPause = value;
            OnPropertyChanged();
        }
    }

    private readonly GameViewModel _viewModel;

    public GamePage()
    {
        InitializeComponent();

        BindingContext = _viewModel = new GameViewModel();

        HydraCanvas.PaintSurface += _viewModel.HydraCanvasPaintSurface;
        SpiderCanvas.PaintSurface += _viewModel.SpiderCanvasPaintSurface;
        DeadSpiderCanvas.PaintSurface += _viewModel.DeadSpiderCanvasPaintSurface;

        _viewModel.PageIsActive = false;
        IsPause = true;
    }

    protected override void OnDisappearing()
    {
        if (!_viewModel.PageIsActive) return;

        base.OnDisappearing();
        Task.Run(Pause);
    }

    protected override bool OnBackButtonPressed()
    {
        Task.Run(Pause);

        return true;
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
        _viewModel.SetPosition((float)width, (float)height * 10 / 11);
    }

    private async Task Pause()
    {
        if (!_viewModel.PageIsActive) return;

        _viewModel.PageIsActive = false;

        var result = await Task.Run(() => new PausePopup().ShowAsync());

        switch (result?.Status)
        {
            case DialogReturnStatuses.Positive:
                IsPause = false;
                break;
            case DialogReturnStatuses.Negative:
                await Shell.Current.GoToAsync("..");
                break;
        }
    }

    private void PauseButtonClicked(object sender, EventArgs e)
    {
        Task.Run(Pause);
    }

    private void HydraCanvasTapped(object? sender, TappedEventArgs e)
    {
        if (IsPause)
        {
            IsPause = false;
            return;
        }

        var relativeToContainerPosition = e.GetPosition((View?)sender);
        if (relativeToContainerPosition != null)
        {
            _viewModel.TapPoint = new Vector2((float)relativeToContainerPosition.Value.X,
                (float)relativeToContainerPosition.Value.Y);
        }
    }
}