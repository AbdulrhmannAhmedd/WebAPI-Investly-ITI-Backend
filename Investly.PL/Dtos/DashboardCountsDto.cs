namespace Investly.PL.Dtos
{
    public class DashboardCountsDto
    {
        public int TotalActiveBusinessIdeas { get; set; }
        public int TotalActiveFounders { get; set; }
        public int TotalActiveInvestors { get; set; }
        public int TotalAcceptedContactRequests { get; set; }
    }
    public class TimeBasedAnalyticsDto
    {
        public int FoundersJoined { get; set; }
        public int InvestorsJoined { get; set; }
        public int IdeasSubmitted { get; set; }
        public string Timeframe { get; set; } = null!;
    }

    public class MonthlyContactRequestDto
    {
        public int Year { get; set; }
        public int Month { get; set; } 
        public string MonthName { get; set; } = null!;
        public int Count { get; set; }
    }

    public class IdeasByCategoryDto
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public int IdeasCount { get; set; }
    }
    public class MostActiveInvestorDto
    {
        public int InvestorId { get; set; }
        public string? InvestorName { get; set; }
        public int RequestCount { get; set; }
    }

    public class MostSupportedFounderDto
    {
        public int FounderId { get; set; }
        public string? FounderName { get; set; }
        public int SupportedIdeasCount { get; set; }
    }
    public class UserCountByGovernmentDto
    {
        public int GovernmentId { get; set; }
        public string? GovernmentName { get; set; }
        public int UserCount { get; set; }
    }

    public class UserCountByCityDto
    {
        public int CityId { get; set; }
        public string? CityName { get; set; }
        public int UserCount { get; set; }
    }
    public class BusinessIdeasByStageDto
    {
        public int Stage { get; set; }
        public string? StageName { get; set; } 
        public int IdeasCount { get; set; }
    }

    public class AvgAiRateByCategoryDto
    {
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
        public decimal? AverageAiRate { get; set; }
    }

    public class IdeasByInvestmentTypeDto
    {
        public int InvestmentType { get; set; } 
        public string? InvestmentTypeName { get; set; }
        public int IdeasCount { get; set; }
    }
}
