using Labb3_QuizApp.Command;
using Labb3_QuizApp.Models;

namespace Labb3_QuizApp.ViewModels;

class ConfigurationViewModel : ViewModelBase
{
    private readonly MainWindowViewModel? _mainWindowViewModel;
    public QuestionPackViewModel ActivePack => _mainWindowViewModel.ActivePack;

    public DelegateCommand AddNewQuestionCommand { get; }
    
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
        AddNewQuestionCommand = new DelegateCommand(AddNewQuestion, CanAddNewQuestion);
    }

    public void AddNewQuestion(object? arg)
    {
        ActivePack.Questions.Add(new Question("Test new", "Correct test", "Incorrect1", "Incorrect2", "incorrect3"));
    }
    public bool CanAddNewQuestion(object? arg)
    {
        return true;
    }
}
