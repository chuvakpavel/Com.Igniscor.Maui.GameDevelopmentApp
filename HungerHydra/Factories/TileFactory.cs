using HungerHydra.Models.TileModels;
using SkiaSharp;

namespace HungerHydra.Factories
{
    internal static class TileFactory
    {
        internal static Tile CreateTile(int tileWidth, int tileHeight, int widthInTiles, int heightInTiles, int number)
        {
            var xTile = number;
            var yTile = 0;

            while (xTile >= widthInTiles)
            {
                xTile -= widthInTiles;
                yTile++;
                if (xTile < widthInTiles)
                {
                    break;
                }

                if (yTile > heightInTiles)
                {
                    return Tile.Empty();
                }
            }

            float left = xTile * tileWidth;
            float top = yTile * tileHeight;
            float right = left + tileWidth;
            float bottom = top + tileHeight;

            return new Tile(tileWidth, tileHeight, new SKRect(left, top, right, bottom));
        }

    }
}
