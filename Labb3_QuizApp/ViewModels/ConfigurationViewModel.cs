using Labb3_QuizApp.Command;
using Labb3_QuizApp.Models;
using Labb3_QuizApp.Services;
using Labb3_QuizApp.Windows;

namespace Labb3_QuizApp.ViewModels;

class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public QuestionPackViewModel ActivePack => _mainWindowViewModel.ActivePack;

    public DelegateCommand OpenOptionsWindowCommand { get; }

    public OptionsViewModel OptionsViewModel { get; }

    public DelegateCommand AddNewQuestionCommand { get; }
    public DelegateCommand RemoveSelectedQuestionCommand { get; }
    public PackHandlerService PackHandlerService { get; }



    private Question _selectedQuestion;
    public Question SelectedQuestion
    {
        get => _selectedQuestion;
        set
        {
            _selectedQuestion = value;
            RaisePropertyChanged();
        }
    }
    public ConfigurationViewModel(MainWindowViewModel? mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        OptionsViewModel = new OptionsViewModel(this);
        OpenOptionsWindowCommand = new DelegateCommand(OpenActivePackOptions);

        AddNewQuestionCommand = new DelegateCommand(AddNewQuestion, CanAddNewQuestion);
        RemoveSelectedQuestionCommand = new DelegateCommand(RemoveSelectedQuestion, CanRemoveSelectedQuestion);
        PackHandlerService = new PackHandlerService();

    }

    public void AddNewQuestion(object? arg)
    {
        ActivePack.Questions.Add(new Question("Test new", "Correct test", "Incorrect1", "Incorrect2", "incorrect3"));
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
        if (arg is Question)
        {
            return true;
        }
        return false;
    }

    public void OpenActivePackOptions(object? arg)
    {

        var optionsWindow = new OptionsWindow();
        optionsWindow.DataContext = ActivePack;
        optionsWindow.ShowDialog();

    }
}
