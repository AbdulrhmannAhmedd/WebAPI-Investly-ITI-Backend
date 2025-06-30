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

        public async Task<string> EvaluateIdea(List<BusinessStandardAnswerDto> businessStandardAnswers, List<StandardCategoryDto> standards)
        {
            string prompt = BuildPrompt(businessStandardAnswers, standards);
            return await CallOpenAiAsync(prompt);



        }


        private string BuildPrompt(List<BusinessStandardAnswerDto> businessStandardAnswers, List<StandardCategoryDto> standards)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("You are a business idea evaluator AI assistant.");
            sb.AppendLine("You will assess the provided user answers for each business evaluation standard.");
            sb.AppendLine("Each standard has a predefined weight representing its importance (integer percentage, no decimals).");
            sb.AppendLine("For each standard:");
            sb.AppendLine("- Analyze the user's answer.");
            sb.AppendLine("- Determine how well the answer meets the standard as an integer percentage (Achievement Score, from 0 to 100).");
            sb.AppendLine("- Calculate the weighted contribution as: (Achievement Score × Weight) ÷ 100, then round the result to the nearest whole number (integer, no decimals).");
            sb.AppendLine("- Provide specific, actionable feedback for each standard.");
            sb.AppendLine("- Include the 'standardCategoryId' and 'name' in the JSON response. The 'name' refers to the Standard Name provided for each standard.");

            sb.AppendLine("\nStandards and User Answers:");

            foreach (var s in standards)
            {
                var answer = businessStandardAnswers.FirstOrDefault(x => x.StandardId == s.StandardId);
                sb.AppendLine($"- \"{s.Question}\" ({s.StandardCategoryWeight}% weight) [standardCategoryId: {s.Id}] [name: \"{s.StandardName}\"]");
                sb.AppendLine($"  User Answer: \"{answer?.Answer ?? "No answer provided"}\"");
            }

            sb.AppendLine("\nProvide the evaluation strictly in the following valid JSON format:");
            sb.AppendLine(@"
{
  ""standards"": [
    {
      ""standardCategoryId"": 3,
      ""name"": ""Standard Name Here"",
      ""weight"": 20,                       
      ""achievementScore"": 70,            
      ""weightedContribution"": 14,        
      ""feedback"": ""Specific, actionable feedback.""
    }
  ],
  ""totalWeightedScore"": 71,              
  ""generalFeedback"": ""Overall assessment and improvement suggestions.""
}
");

            sb.AppendLine("Ensure 'weight', 'achievementScore', 'weightedContribution', and 'totalWeightedScore' are all integers with no decimals. Do not include any text outside the JSON block. Ensure valid JSON formatting for deserialization.");

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
