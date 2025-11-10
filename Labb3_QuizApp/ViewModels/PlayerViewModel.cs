using Labb3_QuizApp.Command;
using System.Windows;
using System.Windows.Threading;

namespace Labb3_QuizApp.ViewModels;

class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public DelegateCommand SetPackNameCommand { get; }
    public DelegateCommand PlayGameCommand { get; }
    public QuestionPackViewModel ActivePack { get => _mainWindowViewModel.ActivePack; }

    private int _questionSet = 0;

    private int _timerText = 30;

    public int TimerText
    {
        get => _timerText; 
        set 
        { 
            _timerText = value;
            RaisePropertyChanged();
        }
    }

    private List<QuestionViewModel> _questionViewModels;

    private string _currentQuestionOutOfTotal = "";
    public string CurrentQuestionOutOfTotal
    {
        get => _currentQuestionOutOfTotal;
        set
        {
            _currentQuestionOutOfTotal = value;
            RaisePropertyChanged();
        }

    }
    public QuestionViewModel CurrentQuestion
    {
        get
        {
            /*
            if (_questionViewModels != null && 
                QuestionSet >= 0 && 
                QuestionSet < _questionViewModels.Count)
            {
                return _questionViewModels[QuestionSet];
            }*/
            _questionViewModels = ActivePack.Questions
                .Select(q => new QuestionViewModel(q))
                .ToList();
            var questions = _questionViewModels[QuestionSet];
            Random random = new Random();
            int maxSelectionAvailable = 4;

            return new QuestionViewModel(ActivePack.Questions.FirstOrDefault(q => q.Query is not null));
        }
    }
    public int QuestionSet
    {
        get => _questionSet;
        set
        {
            _questionSet = value;
            RaisePropertyChanged();
        }
    }

    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        PlayGameCommand = new DelegateCommand(PlayGame, CanPlayGame);

    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        TimerText -= 1;
    }

    private void NextQuestionSet()
    {
        if (ActivePack?.Questions != null && QuestionSet < ActivePack.Questions.Count - 1)
        {
            QuestionSet++;
        }
    }

    public void PlayGame(object? arg)
    {

        var timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1.0);
        timer.Tick += Timer_Tick;
        timer.Start();

        _questionViewModels = ActivePack.Questions
            .Select(q => new QuestionViewModel(q))
            .ToList();

        string test = _questionViewModels[0].Query;
        CurrentQuestionOutOfTotal = $"Question {QuestionSet} out of {ActivePack.Questions.Count}";

    }

    private bool CanPlayGame(object? arg)
    {
        return true;
    }
}
