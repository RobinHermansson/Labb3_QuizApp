using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class TriviaApiResponse
{
    public int response_code { get; set; }
    public List<TriviaQuestion> results { get; set; }
}

class TriviaQuestion
{
    public string type { get; set; }
    public string difficulty { get; set; }
    public string category { get; set; }
    public string question { get; set; }
    public string correct_answer { get; set; }
    public List<string> incorrect_answers { get; set; }
}