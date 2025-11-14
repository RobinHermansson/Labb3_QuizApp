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
            string categoryId = "",
            Difficulty difficulty = Difficulty.Medium)
        {

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            };

            string apiParams;
            if (string.IsNullOrEmpty(categoryId) || categoryId == "0")
            {
                apiParams = $"?amount={numberOfQuestions}&difficulty={difficulty.ToString().ToLower()}&type=multiple";
            }
            else
            {
                apiParams = $"?amount={numberOfQuestions}&category={categoryId}&difficulty={difficulty.ToString().ToLower()}&type=multiple";
            }
            
            var response = await _httpClient.GetAsync(apiParams);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<TriviaQuestionApiResponse>(json, options);
            if (apiResponse == null || apiResponse.results == null || apiResponse.results.Count == 0)
            {
                throw new Exception("No questions returned from API");
            }

            string packName;
            if (string.IsNullOrEmpty(categoryId) || categoryId == "0")
            {
                packName = $"Trivia Mix ({difficulty})";
            }
            else
            {
                string categoryName = apiResponse.results.FirstOrDefault()?.category ?? $"Category {categoryId}";
                packName = $"Trivia: {categoryName} ({difficulty})";
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
        public async Task<List<TriviaCategory>> GetCategoriesAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://opentdb.com/api_category.php");
            response.EnsureSuccessStatusCode();
            
            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            
            var categoriesResponse = JsonSerializer.Deserialize<TriviaCategoriesResponse>(json, options);
            return categoriesResponse?.trivia_categories ?? new List<TriviaCategory>();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error fetching categories: {ex.Message}");
            return new List<TriviaCategory>();
        }
    }
    }
}
