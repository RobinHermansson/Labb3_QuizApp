using Labb3_QuizApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

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
        public async Task<QuestionPack> GetQuestionPackAsync()
        {

            var options = new JsonSerializerOptions()
            {
                WriteIndented = true,
                //IncludeFields = true,
                //IgnoreReadOnlyProperties = true,
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            };

            TriviaApiRequestGenerator apiRequestHelper = new TriviaApiRequestGenerator();
            string apiParams = apiRequestHelper.GenerateParamsString();
            var response = await _httpClient.GetAsync(apiParams);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<TriviaApiResponse>(json, options);
            if (apiResponse == null || apiResponse.results == null || apiResponse.results.Count == 0)
            {
                throw new Exception("No questions returned from API");
            }

            var triviaPack = new QuestionPack("From External Source");

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

                //triviaPack.Questions.Add(new Question() { Query = triviaQuestion.question, CorrectAnswer = triviaQuestion.correct_answer, IncorrectAnswers = [triviaQuestion.incorrect_answers] });

            }

            return triviaPack;

        }
    }
}
