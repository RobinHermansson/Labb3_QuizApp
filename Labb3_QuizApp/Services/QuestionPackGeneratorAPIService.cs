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
            _httpClient.BaseAddress = new Uri("https://opentdb.com/api.php?amount=10");
            
        }
        public async Task<QuestionPack> GetQuestionPackAsync()
        {

            var response = await _httpClient.GetAsync(_httpClient.BaseAddress);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<QuestionPack>(json);

        }
    }
}
