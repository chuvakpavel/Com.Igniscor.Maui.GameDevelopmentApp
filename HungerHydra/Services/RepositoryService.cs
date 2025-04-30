using HungerHydra.Models;

namespace HungerHydra.Services;

internal class RepositoryService : IService
{
    #region Instance

    public static IService Instance => _instance ??= new RepositoryService();

    private static IService? _instance;

    #endregion Instance

    private readonly IRepository _repository;

    private RepositoryService()
    {
        _repository = ScoreRepository.Instance;
    }

    public RepositoryValue<List<Score>> GetScores() => _repository.GetScores();

    public RepositoryValue<Score?> GetHighScore() => _repository.GetHighScore();

    public RepositoryValue<Score> AddScore(int score) => _repository.AddScore(score);
    public RepositoryValue<bool> ClearScores() => _repository.ClearScores();
}