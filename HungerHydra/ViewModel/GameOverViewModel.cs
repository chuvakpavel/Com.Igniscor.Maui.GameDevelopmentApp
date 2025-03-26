using HungerHydra.Abstractions;

namespace HungerHydra.ViewModel;

internal class GameOverViewModel : BasePopupViewModel
{
    private string _score;

    public string Score
    {
        get => _score;
        set
        {
            _score = value;
            OnPropertyChanged();
        }
    }

    private string _highScore;

    public string HighScore
    {
        get => _highScore;
        set
        {
            _highScore = value;
            OnPropertyChanged();
        }
    }

    public GameOverViewModel(string score, string highScore)
    {
        Score = score;
        HighScore = highScore;
    }
}