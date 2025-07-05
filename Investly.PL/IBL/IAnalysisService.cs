using Investly.PL.Dtos;
using Investly.PL.General;

namespace Investly.PL.IBL
{
    public interface IAnalysisService
    {
        DashboardCountsDto GetDashboardCounts();
        TimeBasedAnalyticsDto GetTimeBasedAnalytics(TimeframeType timeframe);
        List<MonthlyContactRequestDto> GetMonthlyAcceptedContactRequests();
        List<IdeasByCategoryDto> GetIdeasCountByCategory();
        List<MostActiveInvestorDto> GetMostActiveInvestors(int topN = 5);
        List<MostSupportedFounderDto> GetMostSupportedFounders(int topN = 5);
        List<UserCountByGovernmentDto> GetUserCountsByGovernment();
        List<UserCountByCityDto> GetUserCountsByCity();
        List<BusinessIdeasByStageDto> GetBusinessIdeasCountByStage();
        List<AvgAiRateByCategoryDto> GetAvgAiRatePerCategory();
        List<IdeasByInvestmentTypeDto> GetIdeasCountByInvestmentType();

    }
}
