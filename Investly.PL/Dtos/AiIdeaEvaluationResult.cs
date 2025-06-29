namespace Investly.PL.Dtos
{
    public class AiIdeaEvaluationResult
    {
        public int TotalWeightedScore { get; set; }
        public List<StandardAiResult>? Standards { get; set; }
    }

    public class StandardAiResult
    {
        public string? Name { get; set; }
        public int Weight { get; set; }
        public int AchievementScore { get; set; }
        public int WeightedContribution { get; set; }
        public string? Feedback { get; set; }

    }
}
