using Labb3_QuizApp.Models;
using System.Net.Http;
using System.Text.Json;

namespace Labb3_QuizApp.Services
{
    public class QuestionPackGeneratorAPIService
    {
        private readonly HttpClient _httpClient;


        public QuestionPackGeneratorAPIService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://opentdb.com/api.php");

        }
        public async Task<QuestionPack> GetQuestionPackAsync(
            int numberOfQuestions = 10,
            string category = "Any Category",
            Difficulty difficulty = Difficulty.Medium)
        {

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            };

            TriviaApiRequestGenerator apiRequestHelper = new TriviaApiRequestGenerator();
            string apiParams = apiRequestHelper.GenerateParamsString(numberOfQuestions, category, difficulty);
            var response = await _httpClient.GetAsync(apiParams);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<TriviaApiResponse>(json, options);
            if (apiResponse == null || apiResponse.results == null || apiResponse.results.Count == 0)
            {
                throw new Exception("No questions returned from API");
            }

            string packName = $"Trivia: {category} ({difficulty})";
            if (category == "Any Category")
            {
                packName = $"Trivia Mix ({difficulty})";
            }
            var triviaPack = new QuestionPack(packName);

            foreach (var triviaQuestion in apiResponse.results)
            {
                string decodedQuestion = System.Net.WebUtility.HtmlDecode(triviaQuestion.question);
                string decodedCorrectAnswer = System.Net.WebUtility.HtmlDecode(triviaQuestion.correct_answer);

                string[] incorrectAnswers = triviaQuestion.incorrect_answers
                    .Select(answer => System.Net.WebUtility.HtmlDecode(answer))
                    .ToArray();

                triviaPack.Questions.Add(new Question(
                    decodedQuestion,
                    decodedCorrectAnswer,
                    incorrectAnswers[0],
                    incorrectAnswers[1],
                    incorrectAnswers[2]
                ));
            }

            return triviaPack;

        }
    }
}
