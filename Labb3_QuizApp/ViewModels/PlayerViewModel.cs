using Labb3_QuizApp.Command;
using Labb3_QuizApp.Views;
using System.Windows;
using System.Windows.Controls;

namespace Labb3_QuizApp.ViewModels;

class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public DelegateCommand SetPackNameCommand { get; }
    public DelegateCommand PlayGameCommand { get; }
    public QuestionPackViewModel ActivePack { get => _mainWindowViewModel.ActivePack; }

    private int _questionSet = 0;

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
        SetPackNameCommand = new DelegateCommand(SetPackName, CanSetPackName);
        PlayGameCommand = new DelegateCommand(PlayGame, CanPlayGame);
        DemoText = string.Empty;
        
    }

    private string _demoText;

    public string DemoText
    {
        get => _demoText;
        set
        {
            _demoText = value;
            RaisePropertyChanged();
            SetPackNameCommand.RaiseCanExecuteChanged();
        }
    }

    private bool CanSetPackName(object? arg)
    {
        return DemoText.Length > 0;
    }

    private void SetPackName(object? obj)
    {
        ActivePack.Name = DemoText;
    }

    private void NextQuestionSet()
    {
        if (ActivePack?.Questions != null && QuestionSet < ActivePack.Questions.Count - 1)
        {
            QuestionSet++;
        }
    }

    private void PlayGame(object? arg)
    {
        _questionViewModels = ActivePack.Questions
            .Select(q => new QuestionViewModel(q))
            .ToList();

        string test = _questionViewModels[0].Query;
        CurrentQuestionOutOfTotal = $"Question {QuestionSet} out of {ActivePack.Questions.Count}";
        MessageBox.Show(test);
        
    }

    private bool CanPlayGame(object? arg)
    {
        return true;
    }
}
