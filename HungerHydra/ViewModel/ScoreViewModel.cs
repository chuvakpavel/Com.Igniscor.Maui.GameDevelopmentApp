using CommunityToolkit.Mvvm.Input;
using HungerHydra.Abstractions;
using HungerHydra.Enums;
using HungerHydra.Models;
using HungerHydra.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace HungerHydra.ViewModel;

internal class ScoreViewModel : BaseViewModel
{
    private readonly IService _scoreService;
    private ObservableCollection<Score> _scores;

    public ObservableCollection<Score> Scores
    {
        get => _scores;
        set
        {
            _scores = value;
            OnPropertyChanged();
        }
    }

    public ICommand ClearScoreCommand { get; set; }
    public ICommand GoBackCommand { get; set; }

    public ScoreViewModel()
    {
        _scoreService = RepositoryService.Instance;
        var scoresValue = _scoreService.GetScores();
        Scores = new ObservableCollection<Score>();
        if (scoresValue.Status == RepositoryStatuses.Found)
        {
            foreach (var score in scoresValue.Value)
            {
                Scores.Add(score);
            }
        }
        ClearScoreCommand = new Command(ClearScore);
        GoBackCommand = new AsyncRelayCommand(GoBack);
    }

    private void ClearScore()
    {
        _scoreService.ClearScores();
        Scores.Clear();
    }

    private async Task GoBack()
    {
       await Shell.Current.GoToAsync("..");
    }
}