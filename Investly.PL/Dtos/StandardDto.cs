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

        public  ICollection<BusinessStandardAnswerDto> BusinessStandardAnswers { get; set; } = new List<BusinessStandardAnswerDto>();

        public  UserDto? CreatedByNavigation { get; set; }

        public  UserDto? UpdatedByNavigation { get; set; }
    }



}
