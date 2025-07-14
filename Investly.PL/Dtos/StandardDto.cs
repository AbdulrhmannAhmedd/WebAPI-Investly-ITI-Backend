using Investly.DAL.Entities;

namespace Investly.PL.Dtos
{
    public class StandardDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? FormQuestion { get; set; }

        public int? Status { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<BusinessStandardAnswerDto> BusinessStandardAnswers { get; set; } = new List<BusinessStandardAnswerDto>();

        public UserDto? CreatedByNavigation { get; set; }

        public UserDto? UpdatedByNavigation { get; set; }
    }
    public class StandardCategoryDto
    {
        public int Id { get; set; }
        public string? StandardName { get; set; }
        public string? Question { get; set; }
        public int StandardId { get; set; }
        public int StandardCategoryWeight { get; set; }

    }
    public class StandardItemAiDto
    {
        public string Standard { get; set; }
        public int Weight { get; set; }
        public string Question { get; set; }
    }



}
