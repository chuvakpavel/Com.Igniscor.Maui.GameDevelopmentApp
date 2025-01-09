using HungerHydra.Factories;
using HungerHydra.Helpers;
using HungerHydra.Models.TileModels;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace HungerHydra.Views;

public partial class MainPage
{
    private int _animationIndex;

    private int AnimationIndex
    {
        get => _animationIndex;
        set => _animationIndex = _animationIndex < _tileSet.TilesCount - 1 ? value : 0;
    }

    private readonly TileSet _tileSet;

    private const int TileSize = 256;
    private const float AnimationCycleTime = 33.3f;

    private bool _pageIsActive;

    public MainPage()
    {
        _pageIsActive = false;
        InitializeComponent();

        var imageData = Task.Run(() => FileReader.GetImageData(Constants.Images.ZeroHydraWalk)).Result;

        _tileSet = TileSetFactory.CreateTileSet(TileSize, TileSize, imageData);

        AnimationIndex = 0;

        HydraCanvas.PaintSurface += HydraCanvasPaintSurface;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _pageIsActive = true;

        Dispatcher.StartTimer(TimeSpan.FromMilliseconds(AnimationCycleTime), () =>
        {
            HydraCanvas.InvalidateSurface();

            AnimationIndex++;
            
            return _pageIsActive;
        });
    }

    private void HydraCanvasPaintSurface(object? sender, SKPaintSurfaceEventArgs args)
    {
        var surface = args.Surface;
        var canvas = surface.Canvas;

        canvas.Clear();

        canvas.DrawBitmap(_tileSet.TilesBitmap, _tileSet.TilesData[AnimationIndex].TileRect,
            new SKRect(0, 0, TileSize * 4, TileSize * 4));
    }
}