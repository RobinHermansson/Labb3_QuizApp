using Labb3_QuizApp.Models;
namespace Labb3_QuizApp.ViewModels;

class QuestionViewModel : ViewModelBase
{

    private Question _question; 
    public Question Question
    {
        get => _question;
        set
        {
            _question = value;
            RaisePropertyChanged();
            /*
            RaisePropertyChanged(nameof(Query));
            RaisePropertyChanged(nameof(CorrectAnswer));
            RaisePropertyChanged(nameof(IncorrectAnswers));
            */
        }
    }

    public string Query => _question.Query;

    public string CorrectAnswer => _question.CorrectAnswer;

    public string[] IncorrectAnswers => _question.IncorrectAnswers;
    public QuestionViewModel(Question model)
    {
        _question = model;
    }

}
