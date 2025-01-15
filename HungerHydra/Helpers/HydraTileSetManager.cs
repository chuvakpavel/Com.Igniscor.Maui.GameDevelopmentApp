using HungerHydra.Abstractions;
using HungerHydra.Enums;
using HungerHydra.Models.TileModels;
using static HungerHydra.Helpers.Constants.Images;

namespace HungerHydra.Helpers;

internal class HydraTileSetManager : BaseTileSetManager
{
    private HydraAnimationStates _currentState;

    private static readonly string[] TileSetsImagePaths = new[]
    {
        ZeroHydraWalk,
        FortyFiveHydraWalk,
        NinetyHydraWalk,
        OneHundredThirtyFiveHydraWalk,
        OneHundredEightyHydraWalk,
        TwoHundredTwentyFiveHydraWalk,
        TwoHundredSeventyHydraWalk,
        TreeHundredFiftyHydraWalk,
        ZeroHydraIdle,
        FortyFiveHydraIdle,
        NinetyHydraIdle,
        OneHundredThirtyFiveHydraIdle,
        OneHundredEightyHydraIdle,
        TwoHundredTwentyFiveHydraIdle,
        TwoHundredSeventyHydraIdle,
        TreeHundredFiftyHydraIdle,
        ZeroHydraWalkShadow,
        FortyFiveHydraWalkShadow,
        NinetyHydraWalkShadow,
        OneHundredThirtyFiveHydraWalkShadow,
        OneHundredEightyHydraWalkShadow,
        TwoHundredTwentyFiveHydraWalkShadow,
        TwoHundredSeventyHydraWalkShadow,
        TreeHundredFiftyHydraWalkShadow,
        ZeroHydraIdleShadow,
        FortyFiveHydraIdleShadow,
        NinetyHydraIdleShadow,
        OneHundredThirtyFiveHydraIdleShadow,
        OneHundredEightyHydraIdleShadow,
        TwoHundredTwentyFiveHydraIdleShadow,
        TwoHundredSeventyHydraIdleShadow,
        TreeHundredFiftyHydraIdleShadow,
        ZeroHydraAttack,
        FortyFiveHydraAttack,
        NinetyHydraAttack,
        OneHundredThirtyFiveHydraAttack,
        OneHundredEightyHydraAttack,
        TwoHundredTwentyFiveHydraAttack,
        TwoHundredSeventyHydraAttack,
        TreeHundredFiftyHydraAttack,
        ZeroHydraAttackShadow,
        FortyFiveHydraAttackShadow,
        NinetyHydraAttackShadow,
        OneHundredThirtyFiveHydraAttackShadow,
        OneHundredEightyHydraAttackShadow,
        TwoHundredTwentyFiveHydraAttackShadow,
        TwoHundredSeventyHydraAttackShadow,
        TreeHundredFiftyHydraAttackShadow
    };
    public HydraTileSetManager(int tileWidth, int tileHeight) : base(TileSetsImagePaths, tileWidth, tileHeight)
    {
    }

    public (TileSet?, TileSet?) GetWalkAnimationTileSets(int xDirection, int yDirection)
    {
        SetWalkState(xDirection, yDirection);

        return _currentState switch
        {
            HydraAnimationStates.ZeroDegreesWalk => (TileSets[FileNames.FindIndex(str => str == ZeroHydraWalk)],
                TileSets[FileNames.FindIndex(str => str == ZeroHydraWalkShadow)]),

            HydraAnimationStates.FortyFiveDegreesWalk => (
                TileSets[FileNames.FindIndex(str => str == FortyFiveHydraWalk)],
                TileSets[FileNames.FindIndex(str => str == FortyFiveHydraWalkShadow)]),

            HydraAnimationStates.NinetyDegreesWalk => (TileSets[FileNames.FindIndex(str => str == NinetyHydraWalk)],
                TileSets[FileNames.FindIndex(str => str == NinetyHydraWalkShadow)]),

            HydraAnimationStates.OneHundredThirtyFiveDegreesWalk => (
                TileSets[FileNames.FindIndex(str => str == OneHundredThirtyFiveHydraWalk)],
                TileSets[FileNames.FindIndex(str => str == OneHundredThirtyFiveHydraWalkShadow)]),

            HydraAnimationStates.OneHundredEightyDegreesWalk => (
                TileSets[FileNames.FindIndex(str => str == OneHundredEightyHydraWalk)],
                TileSets[FileNames.FindIndex(str => str == OneHundredEightyHydraWalkShadow)]),

            HydraAnimationStates.TwoHundredTwentyFiveDegreesWalk => (
                TileSets[FileNames.FindIndex(str => str == TwoHundredTwentyFiveHydraWalk)],
                TileSets[FileNames.FindIndex(str => str == TwoHundredTwentyFiveHydraWalkShadow)]),

            HydraAnimationStates.TwoHundredSeventyDegreesWalk => (
                TileSets[FileNames.FindIndex(str => str == TwoHundredSeventyHydraWalk)],
                TileSets[FileNames.FindIndex(str => str == TwoHundredSeventyHydraWalkShadow)]),

            HydraAnimationStates.TreeHundredFiftyDegreesWalk => (
                TileSets[FileNames.FindIndex(str => str == TreeHundredFiftyHydraWalk)],
                TileSets[FileNames.FindIndex(str => str == TreeHundredFiftyHydraWalkShadow)]),

            HydraAnimationStates.ZeroDegreesIdle => (TileSets[FileNames.FindIndex(str => str == ZeroHydraIdle)],
                TileSets[FileNames.FindIndex(str => str == ZeroHydraIdleShadow)]),

            HydraAnimationStates.FortyFiveDegreesIdle => (
                TileSets[FileNames.FindIndex(str => str == FortyFiveHydraIdle)],
                TileSets[FileNames.FindIndex(str => str == FortyFiveHydraIdleShadow)]),

            HydraAnimationStates.NinetyDegreesIdle => (TileSets[FileNames.FindIndex(str => str == NinetyHydraIdle)],
                TileSets[FileNames.FindIndex(str => str == NinetyHydraIdleShadow)]),

            HydraAnimationStates.OneHundredThirtyFiveDegreesIdle => (
                TileSets[FileNames.FindIndex(str => str == OneHundredThirtyFiveHydraIdle)],
                TileSets[FileNames.FindIndex(str => str == OneHundredThirtyFiveHydraIdleShadow)]),

            HydraAnimationStates.OneHundredEightyDegreesIdle => (
                TileSets[FileNames.FindIndex(str => str == OneHundredEightyHydraIdle)],
                TileSets[FileNames.FindIndex(str => str == OneHundredEightyHydraIdleShadow)]),

            HydraAnimationStates.TwoHundredTwentyFiveDegreesIdle => (
                TileSets[FileNames.FindIndex(str => str == TwoHundredTwentyFiveHydraIdle)],
                TileSets[FileNames.FindIndex(str => str == TwoHundredTwentyFiveHydraIdleShadow)]),

            HydraAnimationStates.TwoHundredSeventyDegreesIdle => (
                TileSets[FileNames.FindIndex(str => str == TwoHundredSeventyHydraIdle)],
                TileSets[FileNames.FindIndex(str => str == TwoHundredSeventyHydraIdleShadow)]),

            HydraAnimationStates.TreeHundredFiftyDegreesIdle => (
                TileSets[FileNames.FindIndex(str => str == TreeHundredFiftyHydraIdle)],
                TileSets[FileNames.FindIndex(str => str == TreeHundredFiftyHydraIdleShadow)]),

            _ => (null, null)
        };
    }

    public (TileSet, TileSet) GetIdleAnimationTileSets()
    {
        return (TileSets[FileNames.FindIndex(str => str == ZeroHydraIdle)],
            TileSets[FileNames.FindIndex(str => str == ZeroHydraIdleShadow)]);
    }

    public (TileSet, TileSet) GetAttackAnimationTileSets(int xDirection, int yDirection)
    {
        SetAttackState(xDirection, yDirection);

        return _currentState switch
        {
            HydraAnimationStates.ZeroDegreesAttack => (TileSets[FileNames.FindIndex(str => str == ZeroHydraAttack)],
                TileSets[FileNames.FindIndex(str => str == ZeroHydraAttackShadow)]),
            HydraAnimationStates.FortyFiveDegreesAttack => (
                TileSets[FileNames.FindIndex(str => str == FortyFiveHydraAttack)],
                TileSets[FileNames.FindIndex(str => str == FortyFiveHydraAttackShadow)]),
            HydraAnimationStates.NinetyDegreesAttack => (
                TileSets[FileNames.FindIndex(str => str == NinetyHydraAttack)],
                TileSets[FileNames.FindIndex(str => str == NinetyHydraAttackShadow)]),
            HydraAnimationStates.OneHundredThirtyFiveDegreesAttack => (
                TileSets[FileNames.FindIndex(str => str == OneHundredThirtyFiveHydraAttack)],
                TileSets[FileNames.FindIndex(str => str == OneHundredThirtyFiveHydraAttackShadow)]),
            HydraAnimationStates.OneHundredEightyDegreesAttack => (
                TileSets[FileNames.FindIndex(str => str == OneHundredEightyHydraAttack)],
                TileSets[FileNames.FindIndex(str => str == OneHundredEightyHydraAttackShadow)]),
            HydraAnimationStates.TwoHundredTwentyFiveDegreesAttack => (
                TileSets[FileNames.FindIndex(str => str == TwoHundredTwentyFiveHydraAttack)],
                TileSets[FileNames.FindIndex(str => str == TwoHundredTwentyFiveHydraAttackShadow)]),
            HydraAnimationStates.TwoHundredSeventyDegreesAttack => (
                TileSets[FileNames.FindIndex(str => str == TwoHundredSeventyHydraAttack)],
                TileSets[FileNames.FindIndex(str => str == TwoHundredSeventyHydraAttackShadow)]),
            HydraAnimationStates.TreeHundredFiftyDegreesAttack => (
                TileSets[FileNames.FindIndex(str => str == TreeHundredFiftyHydraAttack)],
                TileSets[FileNames.FindIndex(str => str == TreeHundredFiftyHydraAttackShadow)]),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private void SetAttackState(int xDirection, int yDirection)
    {
        _currentState = yDirection switch
        {
            < 0 when xDirection == 0 => HydraAnimationStates.ZeroDegreesAttack,
            < 0 when xDirection > 0 => HydraAnimationStates.FortyFiveDegreesAttack,
            0 when xDirection > 0 => HydraAnimationStates.NinetyDegreesAttack,
            > 0 when xDirection > 0 => HydraAnimationStates.OneHundredThirtyFiveDegreesAttack,
            > 0 when xDirection == 0 => HydraAnimationStates.OneHundredEightyDegreesAttack,
            > 0 => HydraAnimationStates.TwoHundredTwentyFiveDegreesAttack,
            0 => HydraAnimationStates.TwoHundredSeventyDegreesAttack,
            < 0 => HydraAnimationStates.TreeHundredFiftyDegreesAttack
        };
    }

    private void SetWalkState(int xDirection, int yDirection)
    {
        switch (yDirection)
        {
            case 0 when xDirection == 0:
                SetIdleState(_currentState);
                return;
            case < 0 when xDirection == 0:
                _currentState = HydraAnimationStates.ZeroDegreesWalk;
                break;
            case < 0 when xDirection > 0:
                _currentState = HydraAnimationStates.FortyFiveDegreesWalk;
                break;
            case 0 when xDirection > 0:
                _currentState = HydraAnimationStates.NinetyDegreesWalk;
                break;
            case > 0 when xDirection > 0:
                _currentState = HydraAnimationStates.OneHundredThirtyFiveDegreesWalk;
                break;
            case > 0 when xDirection == 0:
                _currentState = HydraAnimationStates.OneHundredEightyDegreesWalk;
                break;
            case > 0:
                _currentState = HydraAnimationStates.TwoHundredTwentyFiveDegreesWalk;
                break;
            case 0:
                _currentState = HydraAnimationStates.TwoHundredSeventyDegreesWalk;
                break;
            case < 0:
                _currentState = HydraAnimationStates.TreeHundredFiftyDegreesWalk;
                break;
        }
    }

    private void SetIdleState(HydraAnimationStates previousState)
    {
        _currentState = previousState switch
        {
            HydraAnimationStates.ZeroDegreesWalk or HydraAnimationStates.ZeroDegreesAttack => HydraAnimationStates
                .ZeroDegreesIdle,
            HydraAnimationStates.FortyFiveDegreesWalk or HydraAnimationStates.FortyFiveDegreesAttack =>
                HydraAnimationStates.FortyFiveDegreesIdle,
            HydraAnimationStates.NinetyDegreesWalk or HydraAnimationStates.NinetyDegreesAttack => HydraAnimationStates
                .NinetyDegreesIdle,
            HydraAnimationStates.OneHundredThirtyFiveDegreesWalk
                or HydraAnimationStates.OneHundredThirtyFiveDegreesAttack =>
                HydraAnimationStates.OneHundredThirtyFiveDegreesIdle,
            HydraAnimationStates.OneHundredEightyDegreesWalk or HydraAnimationStates.OneHundredEightyDegreesAttack =>
                HydraAnimationStates.OneHundredEightyDegreesIdle,
            HydraAnimationStates.TwoHundredTwentyFiveDegreesWalk
                or HydraAnimationStates.TwoHundredTwentyFiveDegreesAttack =>
                HydraAnimationStates.TwoHundredTwentyFiveDegreesIdle,
            HydraAnimationStates.TwoHundredSeventyDegreesWalk or HydraAnimationStates.TwoHundredSeventyDegreesAttack =>
                HydraAnimationStates.TwoHundredSeventyDegreesIdle,
            HydraAnimationStates.TreeHundredFiftyDegreesWalk or HydraAnimationStates.TreeHundredFiftyDegreesAttack =>
                HydraAnimationStates.TreeHundredFiftyDegreesIdle,
            _ => previousState
        };
    }
}