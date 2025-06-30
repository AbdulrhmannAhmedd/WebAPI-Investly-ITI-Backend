using Investly.PL.Dtos;
using Investly.PL.General.Services.IServices;
using System.Text;
using System.Text.Json;

namespace Investly.PL.General.Services
{
    public class AiService : IAiService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public AiService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;

        }

        public async Task<string> EvaluateIdea(string ideaText, List<StandardCategoryDto> standards)
        {
            string prompt = BuildPrompt(ideaText, standards);
            return await CallOpenAiAsync(prompt);



        }


        private string BuildPrompt(string ideaTxt, List<StandardCategoryDto> standards)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("You are a business idea evaluator AI assistant.");
            sb.AppendLine("Assess the following business idea based on the provided standards.");
            sb.AppendLine("Each standard has a predefined weight representing its importance.");
            sb.AppendLine("For each standard, determine how much the idea accomplished as a percentage (Achievement Score), and calculate the weighted contribution as: (Achievement Score × Weight) ÷ 100.");
            sb.AppendLine("Each standard also has a unique 'standardCategoryId' which you must include in the JSON response to help us link feedback to our system.");

            sb.AppendLine("\nBusiness Idea Description:\n");
            sb.AppendLine(ideaTxt);

            sb.AppendLine("\nEvaluation Standards:");
            foreach (var s in standards)
            {
                sb.AppendLine($"- \"{s.Question}\" ({s.StandardCategoryWeight}% weight) [standardCategoryId: {s.Id}]");
            }

            sb.AppendLine("\nProvide the evaluation strictly in the following valid JSON format:");
            sb.AppendLine(@"
{
  ""standards"": [
    {
      ""standardCategoryId"": 3,           // The provided unique ID for this standard
      ""name"": ""Standard Name Here"",
      ""weight"": 20,                      // Predefined weight in percentage
      ""achievementScore"": 70,            // How well the user met this standard (0 to 100%)
      ""weightedContribution"": 14,        // (achievementScore × weight) ÷ 100
      ""feedback"": ""Short constructive feedback.""
    }
  ],
  ""totalWeightedScore"": 71              // Sum of all weighted contributions
}
");

            sb.AppendLine("Do not add any text outside the JSON block. Ensure the JSON is valid and properly formatted for deserialization.");

            return sb.ToString();
        }


        private async Task<string> CallOpenAiAsync(string prompt)
        {
            string apiKey = _configuration["OpenAi:ApiKey"];
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var body = new
            {
                model = "gpt-4o-mini",
                messages = new[]
        {
                new { role = "system", content = "You are a business evaluator AI assistant." },
                new { role = "user", content = prompt }
            },
                temperature = 0.2
            };

            var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);
            var responseBody = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseBody);
            var output = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

            return output ?? "No evaluation generated.";

        }
    }
}
