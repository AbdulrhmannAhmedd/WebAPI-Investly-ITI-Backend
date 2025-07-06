using System;
using System.Collections.Generic;

namespace Investly.PL.Dtos
{
    public class FeedbackDto
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string? Description { get; set; }
        public int UserIdTo { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? UserTypeFromName { get; set; } 
        public string? UserTypeToName { get; set; } 
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
        public string? SearchInput { get; set; } 
        public int? StatusFilter { get; set; } 
        public int? UserTypeFromFilter { get; set; } 
        public int? UserTypeToFilter { get; set; }   
    }

    public class FeedbackCountsDto
    {
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
    }
}
