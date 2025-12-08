using FontAwesome.Sharp;
using Labb3_QuizApp.Command;
using Labb3_QuizApp.Models;
using Labb3_QuizApp.Services;

namespace Labb3_QuizApp.ViewModels;

class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public QuestionPackViewModel ActivePack => _mainWindowViewModel.ActivePack;

    public DelegateCommand OpenOptionsWindowCommand { get; }

    public OptionsViewModel OptionsViewModel { get; }

    public DelegateCommand AddNewQuestionCommand { get; }
    public DelegateCommand RemoveSelectedQuestionCommand { get; }



    private Question _selectedQuestion;
    public Question SelectedQuestion
    {
        get => _selectedQuestion;
        set
        {
            _selectedQuestion = value;
            RaisePropertyChanged();
            RemoveSelectedQuestionCommand.RaiseCanExecuteChanged();

            AreConfigureQuestionTextboxesEnabled = (value != null);
        }
    }
    private bool _areConfigureQuestionTextboxesEnabled = false;
    public bool AreConfigureQuestionTextboxesEnabled
    {
        get { return _areConfigureQuestionTextboxesEnabled; }
        set
        {
            _areConfigureQuestionTextboxesEnabled = value;
            RaisePropertyChanged();
        }
        
    }
    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        OptionsViewModel = new OptionsViewModel(this);
        OpenOptionsWindowCommand = new DelegateCommand(_mainWindowViewModel.OpenActivePackOptions);

        AddNewQuestionCommand = new DelegateCommand(AddNewQuestion, CanAddNewQuestion);
        RemoveSelectedQuestionCommand = new DelegateCommand(RemoveSelectedQuestion, CanRemoveSelectedQuestion);

    }

    public void AddNewQuestion(object? arg)
    {
        ActivePack.Questions.Add(new Question("<New Question>", "", "", "", ""));
    }
    public bool CanAddNewQuestion(object? arg)
    {
        return true;
    }
    public void RemoveSelectedQuestion(object? arg)
    {
        ActivePack.Questions.Remove(SelectedQuestion);
    }
    public bool CanRemoveSelectedQuestion(object? arg)
    {
        return SelectedQuestion != null;
    }


}
