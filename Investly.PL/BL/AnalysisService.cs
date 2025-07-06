using System.Globalization;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;

namespace Investly.PL.BL
{
    public class AnalysisService:IAnalysisService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalysisService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public DashboardCountsDto GetDashboardCounts()
        {
            try
            {
                var totalIdeas = _unitOfWork.AnalysisRepo.GetTotalBusinessIdeasCount((int)BusinessIdeaStatus.Active);
                var totalFounders = _unitOfWork.AnalysisRepo.GetTotalFoundersCount((int)UserStatus.Active);
                var totalInvestors = _unitOfWork.AnalysisRepo.GetTotalInvestorsCount((int)UserStatus.Active);
                var totalAcceptedContactRequests = _unitOfWork.AnalysisRepo.GetTotalAcceptedContactRequestsCount((int)ContactRequestStatus.Accepted);

                return new DashboardCountsDto
                {
                    TotalActiveBusinessIdeas = totalIdeas,
                    TotalActiveFounders = totalFounders,
                    TotalActiveInvestors = totalInvestors,
                    TotalAcceptedContactRequests = totalAcceptedContactRequests
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetDashboardCounts: {ex.Message}");
                return new DashboardCountsDto();
            }
        }
        public TimeBasedAnalyticsDto GetTimeBasedAnalytics(TimeframeType timeframe)
        {
            try
            {
                DateTime endDate = DateTime.UtcNow;
                DateTime startDate;
                string timeframeLabel;

                switch (timeframe)
                {
                    case TimeframeType.LastMonth:
                        startDate = endDate.AddMonths(-1);
                        timeframeLabel = "Last Month";
                        break;
                    case TimeframeType.Last3Months:
                        startDate = endDate.AddMonths(-3);
                        timeframeLabel = "Last 3 Months";
                        break;
                    case TimeframeType.Last6Months:
                        startDate = endDate.AddMonths(-6);
                        timeframeLabel = "Last 6 Months";
                        break;
                    case TimeframeType.LastYear:
                        startDate = endDate.AddYears(-1);
                        timeframeLabel = "Last Year";
                        break;
                    case TimeframeType.AllTime:
                        startDate = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                        timeframeLabel = "All Time";
                        break;
                    default:
                        startDate = endDate.AddMonths(-1); // Default to last month
                        timeframeLabel = "Last Month";
                        break;
                }

                // Ensure endDate includes the full day
                endDate = endDate.Date.AddDays(1).AddTicks(-1);

                var foundersCount = _unitOfWork.AnalysisRepo.GetFoundersJoinedCount((int)UserStatus.Active, startDate, endDate);
                var investorsCount = _unitOfWork.AnalysisRepo.GetInvestorsJoinedCount((int)UserStatus.Active, startDate, endDate);
                var ideasCount = _unitOfWork.AnalysisRepo.GetIdeasSubmittedCount((int)BusinessIdeaStatus.Active, startDate, endDate);

                return new TimeBasedAnalyticsDto
                {
                    FoundersJoined = foundersCount, 
                    InvestorsJoined = investorsCount,
                    IdeasSubmitted = ideasCount,
                    Timeframe = timeframeLabel
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetTimeBasedAnalytics: {ex.Message}");
                return new TimeBasedAnalyticsDto { Timeframe = timeframe.ToString() }; // Return with default values on error
            }
        }

        public List<MonthlyContactRequestDto> GetMonthlyAcceptedContactRequests()
        {
            try
            {
                var monthlyData = _unitOfWork.AnalysisRepo.GetContactRequestsCountByMonth((int)ContactRequestStatus.Accepted).ToList();

                var result = monthlyData.Select(x => new MonthlyContactRequestDto
                {
                    Year = (int)x.GetType().GetProperty("Year").GetValue(x),
                    Month = (int)x.GetType().GetProperty("Month").GetValue(x),
                    Count = (int)x.GetType().GetProperty("Count").GetValue(x),
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((int)x.GetType().GetProperty("Month").GetValue(x))
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMonthlyAcceptedContactRequests: {ex.Message}");
                return new List<MonthlyContactRequestDto>(); // Return empty list on error
            }
        }

        public List<IdeasByCategoryDto> GetIdeasCountByCategory()
        {
            try
            {
                var ideasByCategoryData = _unitOfWork.AnalysisRepo.GetIdeasCountByCategory((int)BusinessIdeaStatus.Active).ToList();

                var result = ideasByCategoryData.Select(x => new IdeasByCategoryDto
                {
                    CategoryId = (int)x.GetType().GetProperty("CategoryId").GetValue(x),
                    CategoryName = (string?)x.GetType().GetProperty("CategoryName").GetValue(x),
                    IdeasCount = (int)x.GetType().GetProperty("IdeasCount").GetValue(x)
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetIdeasCountByCategory: {ex.Message}");
                return new List<IdeasByCategoryDto>();
            }
        }

        public List<MostActiveInvestorDto> GetMostActiveInvestors(int topN = 5)
        {
            try
            {
                var mostActiveInvestorsData = _unitOfWork.AnalysisRepo.GetMostActiveInvestors(topN).ToList();

                var result = mostActiveInvestorsData.Select(x => new MostActiveInvestorDto
                {
                    InvestorId = (int)x.GetType().GetProperty("InvestorId").GetValue(x),
                    InvestorName = (string?)x.GetType().GetProperty("InvestorName").GetValue(x),
                    RequestCount = (int)x.GetType().GetProperty("RequestCount").GetValue(x)
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMostActiveInvestors: {ex.Message}");
                return new List<MostActiveInvestorDto>();
            }
        }

        public List<MostSupportedFounderDto> GetMostSupportedFounders(int topN = 5)
        {
            try
            {
                var mostSupportedFoundersData = _unitOfWork.AnalysisRepo.GetMostSupportedFounders((int)ContactRequestStatus.Accepted, topN).ToList();

                var result = mostSupportedFoundersData.Select(x => new MostSupportedFounderDto
                {
                    FounderId = (int)x.GetType().GetProperty("FounderId").GetValue(x),
                    FounderName = (string?)x.GetType().GetProperty("FounderName").GetValue(x),
                    SupportedIdeasCount = (int)x.GetType().GetProperty("SupportedIdeasCount").GetValue(x)
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetMostSupportedFounders: {ex.Message}");
                return new List<MostSupportedFounderDto>();
            }
        }

        public List<UserCountByGovernmentDto> GetUserCountsByGovernment()
        {
            try
            {
                var userCountsByGovernmentData = _unitOfWork.AnalysisRepo.GetUserCountsByGovernment((int)UserStatus.Active).ToList();

                var result = userCountsByGovernmentData.Select(x => new UserCountByGovernmentDto
                {
                    GovernmentId = (int)x.GetType().GetProperty("GovernmentId").GetValue(x),
                    GovernmentName = (string?)x.GetType().GetProperty("GovernmentName").GetValue(x),
                    UserCount = (int)x.GetType().GetProperty("UserCount").GetValue(x)
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserCountsByGovernment: {ex.Message}");
                return new List<UserCountByGovernmentDto>();
            }
        }

        public List<UserCountByCityDto> GetUserCountsByCity()
        {
            try
            {
                var userCountsByCityData = _unitOfWork.AnalysisRepo.GetUserCountsByCity((int)UserStatus.Active).ToList();

                var result = userCountsByCityData.Select(x => new UserCountByCityDto
                {
                    CityId = (int)x.GetType().GetProperty("CityId").GetValue(x),
                    CityName = (string?)x.GetType().GetProperty("CityName").GetValue(x),
                    UserCount = (int)x.GetType().GetProperty("UserCount").GetValue(x)
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetUserCountsByCity: {ex.Message}");
                return new List<UserCountByCityDto>();
            }
        }
        public List<BusinessIdeasByStageDto> GetBusinessIdeasCountByStage()
        {
            try
            {
                var ideasByStageData = _unitOfWork.AnalysisRepo.GetBusinessIdeasCountByStage((int)BusinessIdeaStatus.Active).ToList();

                var result = ideasByStageData.Select(x => new BusinessIdeasByStageDto
                {
                    Stage = (int)x.GetType().GetProperty("Stage").GetValue(x),
                    StageName = Enum.GetName(typeof(Stage), (int)x.GetType().GetProperty("Stage").GetValue(x)),
                    IdeasCount = (int)x.GetType().GetProperty("IdeasCount").GetValue(x)
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBusinessIdeasCountByStage: {ex.Message}");
                return new List<BusinessIdeasByStageDto>();
            }
        }

        public List<AvgAiRateByCategoryDto> GetAvgAiRatePerCategory()
        {
            try
            {
                var avgAiRateData = _unitOfWork.AnalysisRepo.GetAvgAiRatePerCategory((int)BusinessIdeaStatus.Active).ToList();

                var result = avgAiRateData.Select(x => new AvgAiRateByCategoryDto
                {
                    CategoryId = (int)x.GetType().GetProperty("CategoryId").GetValue(x),
                    CategoryName = (string?)x.GetType().GetProperty("CategoryName").GetValue(x),
                    AverageAiRate = (decimal?)x.GetType().GetProperty("AverageAiRate").GetValue(x)
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAvgAiRatePerCategory: {ex.Message}");
                return new List<AvgAiRateByCategoryDto>();
            }
        }

        public List<IdeasByInvestmentTypeDto> GetIdeasCountByInvestmentType()
        {
            try
            {
                var ideasByInvestmentTypeData = _unitOfWork.AnalysisRepo.GetIdeasCountByInvestmentType((int)BusinessIdeaStatus.Active).ToList();

                var result = ideasByInvestmentTypeData.Select(x => new IdeasByInvestmentTypeDto
                {
                    InvestmentType = (int)x.GetType().GetProperty("InvestmentType").GetValue(x),
                    InvestmentTypeName = Enum.GetName(typeof(DesiredInvestmentType), (int)x.GetType().GetProperty("InvestmentType").GetValue(x)),
                    IdeasCount = (int)x.GetType().GetProperty("IdeasCount").GetValue(x)
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetIdeasCountByInvestmentType: {ex.Message}");
                return new List<IdeasByInvestmentTypeDto>();
            }
        }
    }
}
