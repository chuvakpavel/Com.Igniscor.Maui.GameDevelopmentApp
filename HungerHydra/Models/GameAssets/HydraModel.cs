using HungerHydra.Models.TileModels;
using System.Numerics;
using HungerHydra.Enums;
using HungerHydra.Helpers;
using SkiaSharp;
using static System.Math;

namespace HungerHydra.Models.GameAssets;

internal class HydraModel
{
    private const float TranslationSpeed = 0.5f;
    private const float DeadZoneMax = 6.0f;
    private const float DeadZoneMin = -6.0f;
    private const float HurtBoxWidth = 176.0f;
    private const float HurtBoxHeight = 176.0f;

    private int _yAttackDirection;
    private int _xAttackDirection;

    public int AttackedEnemyId { get; private set; }

    private int _animationIndex;
    private readonly HydraTileSetManager _tileSetManager;


    public HydraState State;
    public (TileSet Body, TileSet Shadow) CurrentTileSets;
    public Vector2 CurrentPoint;

    public readonly float ScaledSize;
    public float Satiety;
    public float XTranslate;
    public float YTranslate;

    internal int AnimationIndex
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
                _animationIndex = 0;
                if (State == HydraState.Attack)
                {
                    State = HydraState.Move;
                }
            }
        }
    }

    private int _xDirection;

    private int XDirection
    {
        get => _xDirection;
        set
        {
            if (_xDirection == value) return;
            _xDirection = value;
            var tileSet = _tileSetManager.GetWalkAnimationTileSets(_xDirection, YDirection);
            if (tileSet is { Item2: not null, Item1: not null })
            {
                CurrentTileSets = tileSet!;
            }
        }
    }

    private int _yDirection;

    private int YDirection
    {
        get => _yDirection;
        set
        {
            if (_yDirection == value) return;
            _yDirection = value;
            var tileSet = _tileSetManager.GetWalkAnimationTileSets(XDirection, _yDirection);
            if (tileSet is { Item2: not null, Item1: not null })
            {
                CurrentTileSets = tileSet!;
            }
        }
    }

    public SKRect HurtBox => new(ScaledSize / 2 - HurtBoxWidth / 2 + XTranslate,
        ScaledSize / 2 - HurtBoxHeight / 2 + YTranslate,
        ScaledSize / 2 + HurtBoxWidth / 2 + XTranslate,
        ScaledSize / 2 + HurtBoxHeight / 2 + YTranslate);


    public HydraModel(float tileSize)
    {
        _tileSetManager = new HydraTileSetManager((int)tileSize, (int)tileSize);
        CurrentTileSets = _tileSetManager.GetIdleAnimationTileSets();
        XDirection = 0;
        YDirection = 0;
        AnimationIndex = 0;
        AttackedEnemyId = -1;
        XTranslate = 0.0f;
        YTranslate = 0.0f;
        ScaledSize = tileSize * 2;
        CurrentPoint = new Vector2(ScaledSize / 2, ScaledSize / 2);
        Satiety = 1.0f;
    }

    public float MoveX(Vector2 tapPoint, float animationCycleTime)
    {
        if (State == HydraState.Move)
        {
            var direction = (tapPoint.X - CurrentPoint.X) / Abs(tapPoint.X - CurrentPoint.X);
            var difference = Abs(tapPoint.X) - Abs(CurrentPoint.X);

            if (difference is < DeadZoneMin or > DeadZoneMax)
            {
                XTranslate += direction * TranslationSpeed * animationCycleTime;

                CurrentPoint.X = (XTranslate + ScaledSize / 2) / (float)DeviceDisplay.MainDisplayInfo.Density;

                XDirection = (int)Ceiling(direction);
            }
            else
            {
                XDirection = 0;
            }
        }

        return XTranslate;
    }

    public float MoveY(Vector2 tapPoint, float animationCycleTime)
    {
        if (State == HydraState.Move)
        {
            var direction = (tapPoint.Y - CurrentPoint.Y) / Abs(tapPoint.Y - CurrentPoint.Y);
            var difference = tapPoint.Y - CurrentPoint.Y;

            if (difference is < DeadZoneMin or > DeadZoneMax)
            {
                YTranslate += direction * TranslationSpeed * animationCycleTime;

                CurrentPoint.Y = (YTranslate + ScaledSize / 2) / (float)DeviceDisplay.MainDisplayInfo.Density;

                YDirection = (int)Ceiling(direction);
            }
            else
            {
                if (XDirection == 0 && YDirection == 0)
                {
                    State = HydraState.Idle;
                    var tileSet = _tileSetManager.GetWalkAnimationTileSets(XDirection, YDirection);
                    if (tileSet is { Item1: not null, Item2: not null })
                    {
                        CurrentTileSets = tileSet!;
                    }
                }

                YDirection = 0;
            }
        }

        return YTranslate;
    }

    public void Attack(Vector2 enemyPosition, int enemyIndex)
    {
        AttackedEnemyId = enemyIndex;
        XDirection = 0;
        YDirection = 0;
        if (State != HydraState.Attack)
        {
            var xDifference = enemyPosition.X - CurrentPoint.X * DeviceDisplay.MainDisplayInfo.Density;
            var yDifference = enemyPosition.Y - CurrentPoint.Y * DeviceDisplay.MainDisplayInfo.Density;

            _xAttackDirection = 0;
            _yAttackDirection = 0;

            if (Abs(xDifference) - Abs(yDifference) is < 70 and > -70)
            {
                _xAttackDirection = (int)(xDifference / Abs(xDifference));
                _yAttackDirection = (int)(yDifference / Abs(yDifference));
            }
            else if (Abs(xDifference) > Abs(yDifference))
            {
                _xAttackDirection = (int)(xDifference / Abs(xDifference));
            }
            else if (Abs(xDifference) < Abs(yDifference))
            {
                _yAttackDirection = (int)(yDifference / Abs(yDifference));
            }

            CurrentTileSets = _tileSetManager.GetAttackAnimationTileSets(_xAttackDirection, _yAttackDirection);
            AnimationIndex = 0;
            State = HydraState.Attack;
        }
    }
}