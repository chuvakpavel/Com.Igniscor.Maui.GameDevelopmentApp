using HungerHydra.Abstractions;
using HungerHydra.Enums;
using HungerHydra.Helpers;
using HungerHydra.Models.GameAssets;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace HungerHydra.ViewModel;

internal class GameViewModel : BaseViewModel
{
#if DEBUG
    private static SKPaint _debugPaint = new SKPaint { Color = new SKColor(255, 0, 0), Style = SKPaintStyle.Stroke };
#endif
    private Vector2 _tapPoint;
    private readonly List<SpiderModel> _spiders;
    private readonly SpiderTileSetManager _spiderTileSetManager;
    private readonly Random _rng;
    private int _spiderCount;

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
    private const float AnimationCycleTime = 45.0f;
    private const double LogicCycleTime = 100.0d; // in milliseconds
    private const double SpawnCycleTime = 1000.0d; // in milliseconds
    private const int SpiderLimit = 5;


    private bool _pageIsActive;

    internal GameViewModel()
    {
        _hydra = new HydraModel(TileSize);
        _spiderTileSetManager = new SpiderTileSetManager(TileSize, TileSize);
        _spiders = new List<SpiderModel>();
        _rng = new Random();
    }

    internal void SetPosition(float width, float height)
    {
        TapPoint = _hydra.CurrentPoint = new Vector2(width / 2, height / 2);
        _hydra.XTranslate = (float)(TapPoint.X * DeviceDisplay.MainDisplayInfo.Density - _hydra.ScaledSize / 2);
        _hydra.YTranslate = (float)(TapPoint.Y * DeviceDisplay.MainDisplayInfo.Density - _hydra.ScaledSize / 2);
        _gameFieldHeight = height;
        _gameFieldWidth = width;
    }

    internal void StartAnimationLoop(BindableObject view, ISKCanvasView hydraCanvas, ISKCanvasView spiderCanvas)
    {
        _pageIsActive = true;

        view.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(AnimationCycleTime), () =>
        {
            hydraCanvas.InvalidateSurface();
            spiderCanvas.InvalidateSurface();

            _hydra.AnimationIndex++;

            return _pageIsActive;
        });
    }

    internal void StartLogicLoop(BindableObject view)
    {
        view.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(LogicCycleTime), () =>
        {
            for (var i = 0; i < _spiders.Count; i++)
            {
                var intersection = SKRect.Intersect(_hydra.HurtBox, _spiders[i].HitBox);

                if (_spiders[i].AnimationIndex >= _spiders[i].CurrentTileSets.Body.TilesCount - 1 &&
                    _spiders[i].CurrentState == SpiderState.Die)
                {
                    _spiders.RemoveAt(i);
                    break;
                }

                if (!intersection.IsEmpty)
                {
                    if (_spiders[i].CurrentState == SpiderState.Idle && _hydra.State != HydraState.Attack &&
                        _spiders[i].Id != _hydra.AttackedEnemyId)
                    {
                        _hydra.Attack(new Vector2(_spiders[i].X + _spiders[i].ScaledSize / 2,
                            _spiders[i].Y + _spiders[i].ScaledSize / 2), _spiders[i].Id);
                    }

                    if (_hydra.AnimationIndex >= _hydra.CurrentTileSets.Body.TilesCount - 5 &&
                        _hydra.State == HydraState.Attack && _spiders[i].Id == _hydra.AttackedEnemyId)
                    {
                        if (_spiders[i].CurrentState != SpiderState.Die)
                        {
                            _spiders[i].Die();
                        }
                    }
                }
            }

            return _pageIsActive;
        });
    }

    internal void StartSpawnLoop(BindableObject view)
    {
        _pageIsActive = true;
        view.Dispatcher.StartTimer(TimeSpan.FromMilliseconds(SpawnCycleTime), () =>
        {
            if (_spiders.Count < SpiderLimit && _gameFieldWidth != 0 && _gameFieldHeight != 0)
            {
                _spiders.Add(new SpiderModel(_spiderTileSetManager)
                {
                    X = _rng.Next(50,
                        (int)(_gameFieldWidth * DeviceDisplay.MainDisplayInfo.Density - TileSize - 50)),
                    Y = _rng.Next(50,
                        (int)(_gameFieldHeight * DeviceDisplay.MainDisplayInfo.Density - TileSize - 50)),
                    CurrentState = SpiderState.Rise,
                    CurrentTileSets = _spiderTileSetManager.GetRiseAnimation(),
                    ScaledSize = TileSize,
                    Id = _spiderCount
                });
                _spiderCount++;
            }

            return _pageIsActive;
        });
    }

    public void SpiderCanvasPaintSurface(object? sender, SKPaintSurfaceEventArgs args)
    {
        var surface = args.Surface;
        var canvas = surface.Canvas;

        canvas.Clear();

        foreach (var spider in _spiders)
        {
            canvas.DrawBitmap(spider.CurrentTileSets.Shadow.TilesBitmap,
                spider.CurrentTileSets.Shadow.TilesData[spider.AnimationIndex].TileRect,
                spider.ScaleRect);
            canvas.DrawBitmap(spider.CurrentTileSets.Body.TilesBitmap,
                spider.CurrentTileSets.Body.TilesData[spider.AnimationIndex].TileRect,
                spider.ScaleRect);
#if DEBUG
            canvas.DrawRect(spider.HitBox, _debugPaint);
#endif

            spider.AnimationIndex++;
        }
    }

    internal void HydraCanvasPaintSurface(object? sender, SKPaintSurfaceEventArgs args)
    {
        var surface = args.Surface;
        var canvas = surface.Canvas;

        var xTranslate = _hydra.MoveX(TapPoint, AnimationCycleTime);
        var yTranslate = _hydra.MoveY(TapPoint, AnimationCycleTime);

        canvas.Clear();

        canvas.DrawBitmap(_hydra.CurrentTileSets.Shadow.TilesBitmap,
            _hydra.CurrentTileSets.Shadow.TilesData[_hydra.AnimationIndex].TileRect,
            new SKRect(xTranslate, yTranslate, _hydra.ScaledSize + xTranslate,
                _hydra.ScaledSize + yTranslate));
        canvas.DrawBitmap(_hydra.CurrentTileSets.Body.TilesBitmap,
            _hydra.CurrentTileSets.Body.TilesData[_hydra.AnimationIndex].TileRect,
            new SKRect(xTranslate, yTranslate, _hydra.ScaledSize + xTranslate,
                _hydra.ScaledSize + yTranslate));

#if DEBUG
        canvas.DrawRect(_hydra.HurtBox, _debugPaint);
#endif
    }
}