using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Entities
{
    public class AiBusinessStandardsEvaluation
    {
        public int Id { get; set; }
        public int BusinessId { get; set; }
        public int CategoryStandardId { get; set; }
        public int? AchievementScore { get; set; }
        public int? Weight { get; set; }
        public int? WeightedContribution { get; set; }
        public string? Feedback { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public virtual Business Business { get; set; } = null!;
        public virtual CategoryStandard CategoryStandard { get; set; } = null!;
    }
}
