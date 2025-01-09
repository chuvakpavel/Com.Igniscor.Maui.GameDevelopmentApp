using HungerHydra.Models.TileModels;
using SkiaSharp;

namespace HungerHydra.Factories
{
    internal static class TileSetFactory
    {
        internal static TileSet CreateTileSet(int tileWidthInPixels, int tileHeightInPixels, byte[] imageSetData)
        {
            var imageBitmap = SKBitmap.Decode(imageSetData);
            var widthInTiles = imageBitmap.Width / tileWidthInPixels;
            var heightInTiles = imageBitmap.Height / tileHeightInPixels;
            var tileData = new List<Tile>();
            while (tileData.Count < widthInTiles * heightInTiles)
            {
                var tile = TileFactory.CreateTile(tileWidthInPixels, tileHeightInPixels, widthInTiles,
                    heightInTiles, tileData.Count);
                if (!tile.IsEmpty)
                {
                    tileData.Add(tile);
                }
            }

            return new TileSet(tileWidthInPixels, tileHeightInPixels, imageBitmap.Width, imageBitmap.Height, tileData,
                imageBitmap);
        }
    }
}
