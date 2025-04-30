using HungerHydra.Abstractions;
using HungerHydra.Enums;
using HungerHydra.Helpers;
using HungerHydra.Models.GameAssets;
using HungerHydra.Popups;
using HungerHydra.Services;
using HungerHydra.Views;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using System.Numerics;

namespace HungerHydra.ViewModel;

internal class GameViewModel : BaseViewModel
{
#if DEBUG
    private static readonly SKPaint DebugPaint = new SKPaint
        { Color = new SKColor(255, 0, 0), Style = SKPaintStyle.Stroke };
#endif
    private Vector2 _tapPoint;
    private readonly List<SpiderModel> _spiders;
    private readonly SpiderTileSetManager _spiderTileSetManager;
    private readonly Random _rng;
    private int _spiderCount;
    private bool _isCombo;

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

    public float Satiety
    {
        get => _hydra.Satiety;
        set
        {
            OnPropertyChanged();
            if (value < 0)
            {
                Task.Run(GameOver);
                _pageIsActive = false;
                return;
            }
            else if (value > MaxSatiety)
            {
                _hydra.Satiety = MaxSatiety;
                return;
            }

            _hydra.Satiety = value;
        }
    }

    private int _score;

    public int Score
    {
        get => _score;
        set
        {
            _score = value;
            OnPropertyChanged();
        }
    }

    private int _factor;

    public int Factor
    {
        get => _factor;
        set
        {
            _factor = value > 5 ? 5 : value;

            if (value != 1)
                ComboProgressBarPercents = 1.0f;
            OnPropertyChanged();
        }
    }

    private float _comboProgressBarPercents;

    public float ComboProgressBarPercents
    {
        get => _comboProgressBarPercents;
        set
        {
            _comboProgressBarPercents = value;
            if (value < 0)
            {
                _comboProgressBarPercents = 0.0f;
                Factor = 1;
                _isCombo = false;
            }

            OnPropertyChanged();
        }
    }

    private float _gameFieldWidth;
    private float _gameFieldHeight;


    private readonly HydraModel _hydra;

    private const int TileSize = 256;
    private const float AnimationCycleTime = 45.0f;
    private const double LogicCycleTime = 100.0d; // in milliseconds
    private const double SpawnCycleTime = 1000.0d; // in milliseconds
    private const int SpiderLimit = 5;
    private const float MaxSatiety = 1.5f;
    private const float SpiderValue = 0.05f;
    private const float StarvePerSecond = 0.0045f;
    private const float ComboPerSecond = 0.032f;

    private bool _pageIsActive;

    internal GameViewModel()
    {
        _hydra = new HydraModel(TileSize);
        _spiderTileSetManager = new SpiderTileSetManager(TileSize, TileSize);
        _spiders = new List<SpiderModel>();
        _rng = new Random();
    }

    private async Task GameOver()
    {
        _pageIsActive = false;

        RepositoryService.Instance.AddScore(_score);
        var scoreRepository = RepositoryService.Instance.GetHighScore();
        if (scoreRepository.Status == RepositoryStatuses.NotFound)
        {
            Console.WriteLine(scoreRepository.Exception);
        }

        if (scoreRepository.Value != null)
        {
            var result =
                await (new GameOverPopup(_score.ToString(), scoreRepository.Value.Value.ToString())).ShowAsync();

            switch (result?.Status)
            {
                case DialogReturnStatuses.Positive:
                    await Reset();
                    break;
                case DialogReturnStatuses.Negative:
                    await Shell.Current.GoToAsync("..");
                    break;
            }
        }
    }

    private async Task Reset()
    {
        await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var page = Shell.Current.Navigation.NavigationStack.LastOrDefault();

            await Shell.Current.GoToAsync(nameof(GamePage));

            Shell.Current.Navigation.RemovePage(page);
        });
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

            Satiety -= StarvePerSecond;
            ComboProgressBarPercents -= ComboPerSecond;

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
                        _hydra.State == HydraState.Attack && _spiders[i].Id == _hydra.AttackedEnemyId &&
                        _spiders[i].CurrentState != SpiderState.Die)
                    {
                        if (_isCombo)
                            Factor++;
                        else
                        {
                            _isCombo = true;
                            ComboProgressBarPercents = 1.0f;
                        }

                        Satiety += SpiderValue * Factor;

                        _spiders[i].Die();
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

            Score += Factor;

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
            canvas.DrawRect(spider.HitBox, DebugPaint);
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
        canvas.DrawRect(_hydra.HurtBox, DebugPaint);
#endif
    }
}