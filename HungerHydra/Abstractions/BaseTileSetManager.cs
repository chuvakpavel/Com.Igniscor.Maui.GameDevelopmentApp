using HungerHydra.Factories;
using HungerHydra.Helpers;
using HungerHydra.Models.TileModels;

namespace HungerHydra.Abstractions;

internal abstract class BaseTileSetManager
{
    protected List<TileSet> TileSets { get; private set; }
    protected readonly List<string> FileNames;

    protected BaseTileSetManager(IEnumerable<string> fileNames, int tileWidth, int tileHeight)
    {
        TileSets = new List<TileSet>();
        FileNames = fileNames.ToList();
        foreach (var fileName in FileNames)
        {
            var imageData = Task.Run(() => FileReader.GetImageData(fileName)).Result;
            TileSets.Add(TileSetFactory.CreateTileSet(tileWidth, tileHeight, imageData));
        }
    }
}