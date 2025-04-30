using HungerHydra.Enums;
using HungerHydra.Models;
using LiteDB;

namespace HungerHydra.Services;

internal class ScoreRepository : IRepository
{
    #region Instance

    public static IRepository Instance => _instance ??= new ScoreRepository();

    private static IRepository? _instance;

    #endregion Instance

    private static readonly string DatabasePath;

    static ScoreRepository()
    {
        DatabasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Score.db");
    }

    private static RepositoryValue<T> Base<T1, T>(Func<ILiteCollection<Score>, T1, T> func, T1 arg)
    {
        var res = new RepositoryValue<T>();

        try
        {
            var dataBase = new LiteDatabase(DatabasePath);
            var collection = dataBase.GetCollection<Score>();

            res.Value = func(collection, arg);

            res.Status = RepositoryStatuses.Found;
            dataBase.Dispose();
        }
        catch (Exception e)
        {
            res.Status = RepositoryStatuses.NotFound;
            res.Exception = e.Message;
        }

        return res;
    }

    public RepositoryValue<List<Score>> GetScores() => Base(
        (collection, _) =>
            collection.FindAll().ToList(), 0);

    public RepositoryValue<Score?> GetHighScore() => Base(
        (collection, _) => collection.FindAll().ToList().MaxBy(x => x.Value)
        , 0);

    public RepositoryValue<Score> AddScore(int score) => Base((collection, i) =>
    {
        var res = new Score(i);
        collection.Insert(new Score(i));
        return res;
    }, score);

    public RepositoryValue<bool> ClearScores() => Base((collection, _) =>
    {
        collection.DeleteAll();
        return true;
    }, 0);
}