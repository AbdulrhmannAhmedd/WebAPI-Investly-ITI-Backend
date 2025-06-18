using Investly.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;

namespace Investly.PL.Dtos
{
    public class BusinessDto
    {
        public int Id { get; set; }
        public int FounderId { get; set; }
        public string? FounderName { get; set; }
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public string Title { get; set; } = null!;
        public decimal? Airate { get; set; }
        public int? Stage { get; set; }
        public string? Location { get; set; }
        public decimal? Capital { get; set; }
        public bool IsDrafted { get; set; }
        public string? FilePath { get; set; }
        public int? Status { get; set; }
        public string? RejectedReason { get; set; }

        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public IFormFile? IdeaFile { get; set; }
        public int? GovernmentId { get; set; }

        public int? CityId { get; set; }
        public string? Description { get; set; }
        public int? DesiredInvestmentType { get; set; }
        public  List<BusinessStandardAnswerDto> BusinessStandardAnswers { get; set; } = new List<BusinessStandardAnswerDto>();
        public CategoryForListDto Category { get; set; } 
        public  CityDto? City { get; set; }
        public  GovernmentDto? Government { get; set; }

    }

    public class BusinessListDto
    {
        public List<BusinessDto> Businesses { get; set; } = new List<BusinessDto>();
        public int TotalCount { get; set; }
    }

    public class BusinessSearchDto
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string? SearchInput { get; set; }
        public int? CategoryId { get; set; }
        public int? FounderId { get; set; } 
        public int? Stage { get; set; }
    }
    public class BusinessCountsDto
    {
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
        public int TotalRejected { get; set; }
        public int TotalPending { get; set; }
    }
}