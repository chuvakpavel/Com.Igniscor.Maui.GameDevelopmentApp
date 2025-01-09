using SkiaSharp;

namespace HungerHydra.Models.TileModels;

internal struct Tile
{
    public readonly int Width;
    public readonly int Height;
    public readonly SKRect TileRect;

    public bool IsEmpty => TileRect.IsEmpty;

    public Tile(int width, int height, SKRect tileData)
    {
        Height = height;
        Width = width;
        TileRect = tileData;
    }

    public static Tile Empty()
    {
        return new Tile();
    }

    public Tile()
    {
        Height = 0;
        Width = 0;
        TileRect = SKRect.Empty;
    }

}