using HungerHydra.Abstractions;
using HungerHydra.Models.TileModels;
using static HungerHydra.Helpers.Constants.Images;

namespace HungerHydra.Helpers;

internal class SpiderTileSetManager : BaseTileSetManager
{
    private static readonly string[] TileSetsPaths = new[]
    {
        ZeroSpiderRise,
        ZeroSpiderIdle,
        ZeroSpiderDie,
        ZeroSpiderRiseShadow,
        ZeroSpiderIdleShadow,
        ZeroSpiderDieShadow
    };

    public SpiderTileSetManager(int tileWidth, int tileHeight) : base(TileSetsPaths, tileWidth, tileHeight)
    {
    }

    public (TileSet, TileSet) GetRiseAnimation() => (TileSets[FileNames.FindIndex(str => str == ZeroSpiderRise)],
        TileSets[FileNames.FindIndex(str => str == ZeroSpiderRiseShadow)]);

    public (TileSet, TileSet) GetIdleAnimation => (TileSets[FileNames.FindIndex(str => str == ZeroSpiderIdle)],
        TileSets[FileNames.FindIndex(str => str == ZeroSpiderIdleShadow)]);

    public (TileSet, TileSet) GetDeathAnimation => (TileSets[FileNames.FindIndex(str => str == ZeroSpiderDie)],
        TileSets[FileNames.FindIndex(str => str == ZeroSpiderDieShadow)]);
}