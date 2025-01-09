namespace HungerHydra.Helpers;

internal class FileReader
{
    public static async Task<byte[]> GetImageData(string fileName)
    {
        var imageData = Array.Empty<byte>();
        try
        {
            await using var stream = await FileSystem.OpenAppPackageFileAsync(fileName);
            using var memStream = new MemoryStream();
            await stream.CopyToAsync(memStream);

            imageData = memStream.ToArray();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return imageData;
    }

}