using Labb3_QuizApp.Models;
using System.Collections.ObjectModel;
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
        }
    }
    public ObservableCollection<AnswerOptionViewModel> AnswerOptions { get; } = new();

    public string Query => _question.Query;

    public string CorrectAnswer => _question.CorrectAnswer;

    private string[] _questionsCombined;
    public string[] QuestionsCombined
    {
        get => _questionsCombined;
        set
        {
            _questionsCombined = value;
            RaisePropertyChanged();
        }
    }
    public string[] IncorrectAnswers => _question.IncorrectAnswers;
    public QuestionViewModel(Question question)
    {
        _question = question;

        var allAnswers = new List<string> { question.CorrectAnswer };
        allAnswers.AddRange(question.IncorrectAnswers);

        var random = new Random();
        var randomized = allAnswers.OrderBy(a => random.Next()).ToList();

        foreach (var answer in randomized)
        {
            AnswerOptions.Add(new AnswerOptionViewModel
            {
                Text = answer,
                IsCorrect = answer == question.CorrectAnswer
            });
        }
    }

}
