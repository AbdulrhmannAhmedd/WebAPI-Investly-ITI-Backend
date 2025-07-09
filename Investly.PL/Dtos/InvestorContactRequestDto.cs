using Investly.DAL.Entities;
using Investly.PL.General;
using System.ComponentModel.DataAnnotations;

namespace Investly.PL.Dtos
{
    public class InvestorContactRequestDto
    {
        public int Id { get; set; }
        // Founder (include the ID if you want to navigate to that specific founder)
        public string FounderName { get; set; }
        public string FounderPhoneNumber { get; set; }
        public string FounderEmail { get; set; }
        public int FounderId { get; set; }
        public string InvestorName{ get; set; }
        public int InvestorId { get; set; }
        public string BusinessTitle { get; set; }
        public int BusinessId { get; set; }
        public ContactRequestStatus Status { get; set; }
        public string? DeclineReason { get; set; }
        public DateTime? CreatedAt { get; set; }
        public  BusinessDtoSeconadary Business { get; set; } 
    }

    public class UpdateContactRequestStatusDto
    {
        public int ContactRequestId { get; set; }

        [Required]
        [Range(1, 3, ErrorMessage = "Status must be Pending (1), Accepted (2), or Declined (3).")]
        public ContactRequestStatus NewStatus { get; set; }
        public string? DeclineReason { get; set; }
    }
    public class CountContactRequestDto
    {
        public int TotalContactRequestCount { get; set; }

        
    }

    public class CreateContactRequestDto
    {
        [Required]
        public int BusinessId { get; set; }
    }

    public class ContactRequestResultDto
    {
        public int? ContactRequestId { get; set; }
    }

}
