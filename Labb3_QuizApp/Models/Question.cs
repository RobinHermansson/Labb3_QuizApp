
namespace Labb3_QuizApp.Models;

public class Question
{
    public Question(string query, string correctAnwer, string incorrectAnswer1, 
        string incorrectAnswer2, string incorrectAnswer3)
    {

        Query = query;
        CorrectAnswer = correctAnwer;
        IncorrectAnswers = [incorrectAnswer1, incorrectAnswer2, incorrectAnswer3];
        
    }

    public Question()
    {
        
    }
    public string Query { get; set; }

    public string CorrectAnswer { get; set; }

    public string[] IncorrectAnswers { get; set; }
}
