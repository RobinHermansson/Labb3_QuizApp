using Labb3_QuizApp.Command;

namespace Labb3_QuizApp.ViewModels;

class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public DelegateCommand SetPackNameCommand { get; }
    public QuestionPackViewModel ActivePack { get => _mainWindowViewModel.ActivePack; }

    private int _questionSet = 0;

    private List<QuestionViewModel> _questionViewModels;
    
    public QuestionViewModel CurrentQuestion
    {
        get
        {
            if (_questionViewModels != null && 
                QuestionSet >= 0 && 
                QuestionSet < _questionViewModels.Count)
            {
                return _questionViewModels[QuestionSet];
            }
            return null;
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
        DemoText = string.Empty;
        PlayGame();
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

    private void PlayGame()
    {
        if (ActivePack?.Questions != null)
        {
            _questionViewModels = ActivePack.Questions
                .Select(q => new QuestionViewModel(q))
                .ToList();
        }
    }
}
