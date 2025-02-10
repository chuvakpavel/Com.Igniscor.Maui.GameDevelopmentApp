using HungerHydra.Enums;
using HungerHydra.Helpers;
using HungerHydra.Models.TileModels;
using SkiaSharp;

namespace HungerHydra.Models.GameAssets;

internal class SpiderModel
{
    private const float HitBoxSize = 125.0f;

    public int Id;
    public required float X { get; init; }
    public required float Y { get; init; }
    public float ScaledSize { get; init; }
    public SpiderState CurrentState { get; set; }
    public required (TileSet Body, TileSet Shadow) CurrentTileSets { get; set; }


    private int _animationIndex;
    private readonly SpiderTileSetManager _tileSetManager;

    public SpiderModel(SpiderTileSetManager tileSetManager)
    {
        _tileSetManager = tileSetManager;
    }

    public int AnimationIndex
    {
        get => _animationIndex;
        set
        {
            if (_animationIndex < CurrentTileSets.Body.TilesCount - 1)
            {
                _animationIndex = value;
            }
            else
            {
                if (CurrentState != SpiderState.Die)
                    _animationIndex = 0;
                if (CurrentState == SpiderState.Rise)
                {
                    CurrentState = SpiderState.Idle;
                    CurrentTileSets = _tileSetManager.GetIdleAnimation;
                }
            }
        }
    }

    public SKRect ScaleRect => new(X, Y, ScaledSize + X, ScaledSize + Y);

    public SKRect HitBox => new(ScaledSize / 2 - HitBoxSize / 2 + X,
        ScaledSize / 2 - HitBoxSize / 2 + Y,
        ScaledSize / 2 + HitBoxSize / 2 + X,
        ScaledSize / 2 + HitBoxSize / 2 + Y);

    public void Die()
    {
        if (CurrentState != SpiderState.Die)
        {
            AnimationIndex = 0;
            CurrentState = SpiderState.Die;
            CurrentTileSets = _tileSetManager.GetDeathAnimation;
        }
    }
}