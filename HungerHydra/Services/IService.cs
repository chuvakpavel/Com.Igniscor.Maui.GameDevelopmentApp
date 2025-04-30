using HungerHydra.Models;

namespace HungerHydra.Services;

internal interface IService
{
    public RepositoryValue<List<Score>> GetScores();

    public RepositoryValue<Score?> GetHighScore();

    public RepositoryValue<Score> AddScore(int score);

    public RepositoryValue<bool> ClearScores();
}