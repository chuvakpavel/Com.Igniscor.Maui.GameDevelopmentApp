using SkiaSharp;

namespace HungerHydra.Models.TileModels;

internal class TileSet
{
    public readonly int HeightInPixels;

    public readonly int WidthInPixels;

    public readonly int TileWidthInPixels;

    public readonly int TileHeightInPixels;

    public int TilesCount => TilesData.Count;

    public readonly List<Tile> TilesData;

    public readonly SKBitmap TilesBitmap;

    public TileSet(int tileWidthInPixels, int tileHeightInPixels, int widthInPixels, int heightInPixels,
        List<Tile> tilesData, SKBitmap tilesBitmap)
    {
        TileHeightInPixels = tileHeightInPixels;
        TileWidthInPixels = tileWidthInPixels;
        TilesData = tilesData;
        WidthInPixels = widthInPixels;
        HeightInPixels = heightInPixels;
        TilesBitmap = tilesBitmap;
    }

}