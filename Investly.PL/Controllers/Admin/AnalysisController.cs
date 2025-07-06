using Investly.PL.Attributes;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    [TypeFilter(typeof(AuthorizeUserTypeAttribute), Arguments = new object[] { (int)UserType.Staff })]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisService _analysisService;

        public AnalysisController(IAnalysisService analysisService)
        {
            _analysisService = analysisService;
        }
        [HttpGet("dashboard-counts")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<DashboardCountsDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto<object>))]
        public ResponseDto<DashboardCountsDto> GetDashboardCounts()
        {
            try
            {
                var counts = _analysisService.GetDashboardCounts();

                if (counts != null)
                {
                    return new ResponseDto<DashboardCountsDto>
                    {
                        IsSuccess = true,
                        Message = "Dashboard counts retrieved successfully.",
                        Data = counts,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ResponseDto<DashboardCountsDto>
                    {
                        IsSuccess = false,
                        Message = "Failed to retrieve dashboard counts.",
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<DashboardCountsDto>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        [HttpGet("time-based-analytics")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<TimeBasedAnalyticsDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto<object>))]
        public ResponseDto<TimeBasedAnalyticsDto> GetTimeBasedAnalytics([FromQuery] TimeframeType timeframe)
        {
            try
            {
                var analytics = _analysisService.GetTimeBasedAnalytics(timeframe);
                if (analytics != null)
                {
                    return new ResponseDto<TimeBasedAnalyticsDto>
                    {
                        IsSuccess = true,
                        Message = $"Time-based analytics for {analytics.Timeframe} retrieved successfully.",
                        Data = analytics,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ResponseDto<TimeBasedAnalyticsDto>
                    {
                        IsSuccess = false,
                        Message = "Failed to retrieve time-based analytics.",
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<TimeBasedAnalyticsDto>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpGet("monthly-accepted-contact-requests")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<List<MonthlyContactRequestDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto<object>))]
        public ResponseDto<List<MonthlyContactRequestDto>> GetMonthlyAcceptedContactRequests()
        {
            try
            {
                var monthlyData = _analysisService.GetMonthlyAcceptedContactRequests();
                if (monthlyData != null)
                {
                    return new ResponseDto<List<MonthlyContactRequestDto>>
                    {
                        IsSuccess = true,
                        Message = "Monthly accepted contact requests retrieved successfully.",
                        Data = monthlyData,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ResponseDto<List<MonthlyContactRequestDto>>
                    {
                        IsSuccess = false,
                        Message = "Failed to retrieve monthly accepted contact requests.",
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<MonthlyContactRequestDto>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        [HttpGet("ideas-by-category")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<List<IdeasByCategoryDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto<object>))]
        public ResponseDto<List<IdeasByCategoryDto>> GetIdeasCountByCategory()
        {
            try
            {
                var ideasByCategory = _analysisService.GetIdeasCountByCategory();
                if (ideasByCategory != null)
                {
                    return new ResponseDto<List<IdeasByCategoryDto>>
                    {
                        IsSuccess = true,
                        Message = "Ideas count by category retrieved successfully.",
                        Data = ideasByCategory,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ResponseDto<List<IdeasByCategoryDto>>
                    {
                        IsSuccess = false,
                        Message = "Failed to retrieve ideas count by category.",
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<IdeasByCategoryDto>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        [HttpGet("most-active-investors")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<List<MostActiveInvestorDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto<object>))]
        public ResponseDto<List<MostActiveInvestorDto>> GetMostActiveInvestors([FromQuery] int topN = 5)
        {
            try
            {
                var investors = _analysisService.GetMostActiveInvestors(topN);
                if (investors != null)
                {
                    return new ResponseDto<List<MostActiveInvestorDto>>
                    {
                        IsSuccess = true,
                        Message = $"Top {topN} most active investors retrieved successfully.",
                        Data = investors,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ResponseDto<List<MostActiveInvestorDto>>
                    {
                        IsSuccess = false,
                        Message = "Failed to retrieve most active investors.",
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<MostActiveInvestorDto>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpGet("most-supported-founders")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<List<MostSupportedFounderDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto<object>))]
        public ResponseDto<List<MostSupportedFounderDto>> GetMostSupportedFounders([FromQuery] int topN = 5)
        {
            try
            {
                var founders = _analysisService.GetMostSupportedFounders(topN);
                if (founders != null)
                {
                    return new ResponseDto<List<MostSupportedFounderDto>>
                    {
                        IsSuccess = true,
                        Message = $"Top {topN} most supported founders retrieved successfully.",
                        Data = founders,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ResponseDto<List<MostSupportedFounderDto>>
                    {
                        IsSuccess = false,
                        Message = "Failed to retrieve most supported founders.",
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<MostSupportedFounderDto>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        [HttpGet("user-counts-by-government")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<List<UserCountByGovernmentDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto<object>))]
        public ResponseDto<List<UserCountByGovernmentDto>> GetUserCountsByGovernment()
        {
            try
            {
                var userCounts = _analysisService.GetUserCountsByGovernment();
                if (userCounts != null)
                {
                    return new ResponseDto<List<UserCountByGovernmentDto>>
                    {
                        IsSuccess = true,
                        Message = "User counts by government retrieved successfully.",
                        Data = userCounts,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ResponseDto<List<UserCountByGovernmentDto>>
                    {
                        IsSuccess = false,
                        Message = "Failed to retrieve user counts by government.",
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<UserCountByGovernmentDto>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpGet("user-counts-by-city")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<List<UserCountByCityDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto<object>))]
        public ResponseDto<List<UserCountByCityDto>> GetUserCountsByCity()
        {
            try
            {
                var userCounts = _analysisService.GetUserCountsByCity();
                if (userCounts != null)
                {
                    return new ResponseDto<List<UserCountByCityDto>>
                    {
                        IsSuccess = true,
                        Message = "User counts by city retrieved successfully.",
                        Data = userCounts,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ResponseDto<List<UserCountByCityDto>>
                    {
                        IsSuccess = false,
                        Message = "Failed to retrieve user counts by city.",
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<UserCountByCityDto>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
        [HttpGet("ideas-by-stage")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<List<BusinessIdeasByStageDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto<object>))]
        public ResponseDto<List<BusinessIdeasByStageDto>> GetBusinessIdeasCountByStage()
        {
            try
            {
                var ideasByStage = _analysisService.GetBusinessIdeasCountByStage();
                if (ideasByStage != null)
                {
                    return new ResponseDto<List<BusinessIdeasByStageDto>>
                    {
                        IsSuccess = true,
                        Message = "Business ideas by stage retrieved successfully.",
                        Data = ideasByStage,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ResponseDto<List<BusinessIdeasByStageDto>>
                    {
                        IsSuccess = false,
                        Message = "Failed to retrieve business ideas by stage.",
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<BusinessIdeasByStageDto>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpGet("avg-ai-rate-by-category")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<List<AvgAiRateByCategoryDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto<object>))]
        public ResponseDto<List<AvgAiRateByCategoryDto>> GetAvgAiRatePerCategory()
        {
            try
            {
                var avgAiRate = _analysisService.GetAvgAiRatePerCategory();
                if (avgAiRate != null)
                {
                    return new ResponseDto<List<AvgAiRateByCategoryDto>>
                    {
                        IsSuccess = true,
                        Message = "Average AI rate by category retrieved successfully.",
                        Data = avgAiRate,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ResponseDto<List<AvgAiRateByCategoryDto>>
                    {
                        IsSuccess = false,
                        Message = "Failed to retrieve average AI rate by category.",
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<AvgAiRateByCategoryDto>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

        [HttpGet("ideas-by-investment-type")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResponseDto<List<IdeasByInvestmentTypeDto>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ResponseDto<object>))]
        public ResponseDto<List<IdeasByInvestmentTypeDto>> GetIdeasCountByInvestmentType()
        {
            try
            {
                var ideasByInvestmentType = _analysisService.GetIdeasCountByInvestmentType();
                if (ideasByInvestmentType != null)
                {
                    return new ResponseDto<List<IdeasByInvestmentTypeDto>>
                    {
                        IsSuccess = true,
                        Message = "Ideas by investment type retrieved successfully.",
                        Data = ideasByInvestmentType,
                        StatusCode = StatusCodes.Status200OK
                    };
                }
                else
                {
                    return new ResponseDto<List<IdeasByInvestmentTypeDto>>
                    {
                        IsSuccess = false,
                        Message = "Failed to retrieve ideas by investment type.",
                        Data = null,
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                }
            }
            catch (Exception ex)
            {
                return new ResponseDto<List<IdeasByInvestmentTypeDto>>
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }
    }
}
