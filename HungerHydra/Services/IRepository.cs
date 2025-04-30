using HungerHydra.Models;

namespace HungerHydra.Services;

internal interface IRepository
{
    public RepositoryValue<List<Score>> GetScores();

    public RepositoryValue<Score?> GetHighScore();

    public RepositoryValue<Score> AddScore(int score);

    public RepositoryValue<bool> ClearScores();
}