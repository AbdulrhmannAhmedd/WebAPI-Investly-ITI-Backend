namespace Investly.PL.Dtos
{
   
    public class AiBusinessEvaluationDto
    {
        public int? BusinessId { get; set; }
        public int? TotalWeightedScore { get; set; }
        public string? GeneralFeedback { get; set; }
        public List<AiStandardsEvaluationDto> Standards { get; set; } = new List<AiStandardsEvaluationDto>();
    }

    public class AiStandardsEvaluationDto
    {
        public int Id { get; set; }
        public int StandardCategoryId { get; set; }
        public int? AchievementScore { get; set; }
        public string? Name { get; set; }
        public string? FormQuestion { get; set; }
        public int? Weight { get; set; }
        public int? WeightedContribution { get; set; }
        public string? Feedback { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }

    }
}
