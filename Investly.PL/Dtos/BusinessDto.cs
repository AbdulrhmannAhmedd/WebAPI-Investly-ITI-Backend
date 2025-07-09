using Investly.DAL.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations.Schema;
using Investly.PL.General;

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
        public string? Images { get; set; }
        public List<IFormFile>? ImageFiles { get; set; } = new List<IFormFile>();
        public int? CityId { get; set; }
        public string? Description { get; set; }
        public int? DesiredInvestmentType { get; set; }
        public string? DesiredInvestmentTypeName { get; set; }
        public CategoryForListDto Category { get; set; } 
        public  CityDto? City { get; set; }
        public  GovernmentDto? Government { get; set; }
        public AiBusinessEvaluationDto? AiBusinessEvaluations { get; set; }
        public  List<BusinessStandardAnswerDto> BusinessStandardAnswers { get; set; } = new List<BusinessStandardAnswerDto>();
        public List<InvestorContactRequestDto> InvestorContactRequests { get; set; } = new List<InvestorContactRequestDto>();


    }




    public class BusinessDtoSeconadary
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
        public string? Images { get; set; }
        public List<IFormFile>? ImageFiles { get; set; } = new List<IFormFile>();
        public int? CityId { get; set; }
        public string? Description { get; set; }
        public int? DesiredInvestmentType { get; set; }
        public string? DesiredInvestmentTypeName { get; set; }
        public CategoryForListDto Category { get; set; }
        public CityDto? City { get; set; }
        public GovernmentDto? Government { get; set; }
        public AiBusinessEvaluationDto? AiBusinessEvaluations { get; set; }
        public List<BusinessStandardAnswerDto> BusinessStandardAnswers { get; set; } = new List<BusinessStandardAnswerDto>();


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

    public class DisplayBusinessToExploreSectionDto
    {
        public int Id { get; set; }
        public string? Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Stage { get; set; }
        public string FounderName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string GovernmentName { get; set; } = null!;
        public decimal Capital { get; set; }
        public decimal Airate { get; set; }
        public int DesiredInvestmentType { get; set; }
        public List<string>? Images { get; set; } = new List<string>();
        public ContactRequestStatus? ContactRequestStatus { get; set; } // nullable to allow for no contact request status (no request made yet)
        public bool CanRequestContact { get; set; } = false;
    }

    public class BusinessListDtoForExplore
    {
        public List<DisplayBusinessToExploreSectionDto> Businesses { get; set; } = new List<DisplayBusinessToExploreSectionDto>();
        public int TotalCount { get; set; }
        public InvestorPreferencesDto? InvestorPreferences { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }

    }

    public class BusinessSeachForExploreDto
    {
        public int PageSize { get; set; } = 10;
        public int PageNumber { get; set; } = 1;
        public string? SearchInput { get; set; }
        public int? CategoryId { get; set; }
        public int? Stage { get; set; }
        public int? GovernmentId { get; set; }
        public int? MinCapital { get; set; }
        public int? MaxCapital { get; set; }
        public int? MinAiRate { get; set; }
        public int? DesiredInvestmentType { get; set; }
        public bool UseDefaultPreferences { get; set; } = true; // true on initial load, false when user interacts..

    }

    public class InvestorPreferencesDto
    {
        public string? InterestedBusinessStages { get; set; }
        public int? MinFunding { get; set; }
        public int? MaxFunding { get; set; }
        public int InvestingType { get; set; }
    }

    public class BusinessDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int Stage { get; set; }
        public string FounderName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public string GovernmentName { get; set; } = null!;
        public string CityName { get; set; } = null!;
        public string Location { get; set; } = null!;
        public decimal Capital { get; set; }
        public decimal Airate { get; set; }
        public int DesiredInvestmentType { get; set; }
        public List<string>? Images { get; set; } = new List<string>();
        public string? FilePath { get; set; }
        public List<BusinessStandardAnswerDto> BusinessStandardAnswers { get; set; } = new List<BusinessStandardAnswerDto>();
        public ContactRequestStatus? ContactRequestStatus { get; set; }
        public bool CanRequestContact { get; set; } = false;
        public int TotalContactRequests { get; set; }
        public bool isInvestor { get; set; } = true;
    }
}