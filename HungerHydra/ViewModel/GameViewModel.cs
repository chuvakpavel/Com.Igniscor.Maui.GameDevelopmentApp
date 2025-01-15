using HungerHydra.Abstractions;
using HungerHydra.Enums;
using HungerHydra.Models.GameAssets;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace HungerHydra.ViewModel;

internal class GameViewModel : BaseViewModel
{
    private Vector2 _tapPoint;

    internal Vector2 TapPoint
    {
        get => _tapPoint;
        set
        {
            _tapPoint = value;
            if (_hydra.State != HydraState.Attack)
            {
                _hydra.State = HydraState.Move;
            }
        }
    }
    private float _gameFieldWidth;
    private float _gameFieldHeight;


    private HydraModel _hydra;

    private const int TileSize = 256;
    private const float AnimationCycleTime = 33.3f;

    private bool _pageIsActive;

    public GameViewModel()
    {
        _pageIsActive = false;

        _hydra = new HydraModel(TileSize);
    }

    internal void SetPosition(float width, float height)
    {
        TapPoint = _hydra.CurrentPoint = new Vector2(width / 2, height / 2);
        _hydra.XTranslate = (float)(TapPoint.X * DeviceDisplay.MainDisplayInfo.Density - _hydra.ScaledSize / 2);
        _hydra.YTranslate = (float)(TapPoint.Y * DeviceDisplay.MainDisplayInfo.Density - _hydra.ScaledSize / 2);
        _gameFieldHeight = height;
        _gameFieldWidth = width;
    }


    internal void StartAnimationLoop(BindableObject view, ISKCanvasView hydraCanvas)
    {
        _pageIsActive = true;

        view.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(AnimationCycleTime), () =>
        {
            hydraCanvas.InvalidateSurface();

            _hydra.AnimationIndex++;

            return _pageIsActive;
        });
    }

    internal void HydraCanvasPaintSurface(object? sender, SKPaintSurfaceEventArgs args)
    {
        var surface = args.Surface;
        var canvas = surface.Canvas;

        var xTranslate = _hydra.MoveX(TapPoint, (float)AnimationCycleTime);
        var yTranslate = _hydra.MoveY(TapPoint, (float)AnimationCycleTime);

        canvas.Clear();

        canvas.DrawBitmap(_hydra.CurrentTileSets.Shadow.TilesBitmap,
            _hydra.CurrentTileSets.Shadow.TilesData[_hydra.AnimationIndex].TileRect,
            new SKRect(xTranslate, yTranslate, _hydra.ScaledSize + xTranslate,
                _hydra.ScaledSize + yTranslate));
        canvas.DrawBitmap(_hydra.CurrentTileSets.Body.TilesBitmap,
            _hydra.CurrentTileSets.Body.TilesData[_hydra.AnimationIndex].TileRect,
            new SKRect(xTranslate, yTranslate, _hydra.ScaledSize + xTranslate,
                _hydra.ScaledSize + yTranslate));
    }
}