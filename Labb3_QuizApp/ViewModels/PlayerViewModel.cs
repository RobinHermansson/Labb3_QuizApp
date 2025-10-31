using Labb3_QuizApp.Command;

namespace Labb3_QuizApp.ViewModels;

class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;

    public DelegateCommand SetPackNameCommand { get; }
    public QuestionPackViewModel ActivePack { get => _mainWindowViewModel.ActivePack; }


    public PlayerViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        SetPackNameCommand = new DelegateCommand(SetPackName, CanSetPackName);
        DemoText = string.Empty;
    }

    private string _demoText;
    
    public string DemoText { get => _demoText;
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
}
