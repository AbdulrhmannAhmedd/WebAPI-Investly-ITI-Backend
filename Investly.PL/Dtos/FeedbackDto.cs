using System;
using System.Collections.Generic;

namespace Investly.PL.Dtos
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public int UserTypeFrom { get; set; }
        public int UserTypeTo { get; set; }
        public int UserIdTo { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }

        // Optionally, include names for display:
        public string? UserTypeFromName { get; set; } // e.g., "Founder"
        public string? UserTypeToName { get; set; }   // e.g., "Investor"
        public string? UserToName { get; set; }
        public string? UserFromName { get; set; }

    }
    public class FeedbackListDto
    {
        public List<FeedbackDto> List { get; set; } = new List<FeedbackDto>();
        public int TotalCount { get; set; }
    }

    public class FeedbackSearchDto
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string? SearchInput { get; set; } // e.g., search by description
        public int? StatusFilter { get; set; } // Filter by specific status (Active, Inactive, Deleted)
        public int? UserTypeFromFilter { get; set; } // Filter by who gave feedback
        public int? UserTypeToFilter { get; set; }   // Filter by who received feedback
    }

    public class FeedbackCountsDto
    {
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
    }
}
