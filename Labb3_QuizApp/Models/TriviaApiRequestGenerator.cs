using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb3_QuizApp.Models;

class TriviaApiRequestGenerator
{

    public int NumberOfQuestions { get; set; }
    public Dictionary<string, string> CategoryDict { get; set; } = new Dictionary<string, string> {
        {"Any Category", ""},
        {"General Knowledge","&category=9"},
        {"Entertainment: Books","&category=10"},
        {"Entertainment: Film","&category=11"},
        {"Entertainment: Music","&category=12"},
        {"Entertainment: Musicals & Theatres","&category=13"},
        {"Entertainment: Television","&category=14"},
        {"Entertainment: Video Games","&category=15"},
        {"Entertainment: Board Games","&category=16"},
        {"Science &amp; Nature","&category=17"},
        {"Science: Computers","&category=18"},
        {"Science: Mathematics","&category=19"},
        {"Mythology","&category=20"},
        {"Sports","&category=21"},
        {"Geography","&category=22"},
        {"History","&category=23"},
        {"Politics","&category=24"},
        {"Art","&category=25"},
        {"Celebrities","&category=26"},
        {"Animals","&category=27"},
        {"Vehicles","&category=28"},
        {"Entertainment: Comics","&category=29"},
        {"Science: Gadgets","&category=30"},
        {"Entertainment: Japanese Anime & Manga","&category=31"},
        {"Entertainment: Cartoon & Animations","&category=32"}
    };

    public Difficulty Difficulty { get; set; }
    private string Type { get; set; } = "multiple";


    public string GenerateParamsString(int numberOfQuestions = 10, string category = "", Difficulty difficulty = Difficulty.Easy)
    {
        var numberOfQuestionsParam = $"amount={numberOfQuestions.ToString()}";
        var categoryParams = "";
        if (category == "")
        {
            categoryParams = "";
        }
        else
        {
            categoryParams = CategoryDict[category];
        }
        var difficultyParams = $"&difficulty={difficulty.ToString().ToLower()}";
        {

            return $"?{numberOfQuestionsParam}{categoryParams}{difficultyParams}&type={Type}";
        }
    }
}

