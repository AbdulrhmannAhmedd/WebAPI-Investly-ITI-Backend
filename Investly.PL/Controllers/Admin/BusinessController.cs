using Investly.PL.Attributes;
using Investly.PL.BL;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Investly.PL.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [AuthorizeUserType(((int)UserType.Staff))]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }

        [HttpGet("All")]
        public ResponseDto<BusinessListDto> GetAllBusinesses([FromQuery] BusinessSearchDto searchDto)
        {
            var businesses = _businessService.GetAllBusinesses(searchDto);

            if (businesses == null || !businesses.Businesses.Any())
            {
                return new ResponseDto<BusinessListDto>
                {
                    IsSuccess = true,
                    Message = "No business ideas found matching the criteria.",
                    Data = businesses,
                    StatusCode = StatusCodes.Status200OK
                };
            }

            return new ResponseDto<BusinessListDto>
            {
                IsSuccess = true,
                Message = "Business ideas retrieved successfully.",
                Data = businesses,
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpPut("{id}/Delete")] 
        public ResponseDto<object> SoftDeleteBusiness(int id)
        {
            int? loggedUserId = null;
            var userIdClaim = User.FindFirst("id"); 
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId))
            {
                loggedUserId = parsedUserId;
            }

            var result = _businessService.SoftDeleteBusiness(id, loggedUserId);
            ResponseDto<object> response;

            if (result > 0)
            {
                response = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Business idea deleted successfully.",
                    Data = null,
                    StatusCode = StatusCodes.Status200OK
                };
            }
            else if (result == -1)
            {
                response = new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Business idea not found.",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            else
            {
                response = new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Failed to delete business idea.",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            return response;
        }

        [HttpPut("{id}/Update")]
      public ResponseDto<object> UpdateBusinessStatus(int id, [FromQuery] BusinessIdeaStatus newStatus, [FromQuery] string? rejectedReason = null)
      {
           int? loggedUserId = null;
           var userIdClaim = User.FindFirst("id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId))
            {
                loggedUserId = parsedUserId;
            }

    var result = _businessService.UpdateBusinessStatus(id, newStatus, loggedUserId, rejectedReason);
    ResponseDto<object> response;

    if (result > 0)
    {
        response = new ResponseDto<object>
        { 
            IsSuccess = true,
            Message = $"Business idea status updated to {newStatus.ToString()} successfully.",
            Data = null,
            StatusCode = StatusCodes.Status200OK
        };
    }
    else if (result == -1)
    {
        response = new ResponseDto<object>
        {
            IsSuccess = false,
            Message = "Business idea not found.",
            Data = null,
            StatusCode = StatusCodes.Status404NotFound
        };
    }
    else if (result == -2)
    {
        response = new ResponseDto<object>
        {
            IsSuccess = false,
            Message = "Invalid status transition for business idea.",
            Data = null,
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
    else if (result == -4)
    {
        response = new ResponseDto<object>
        {
            IsSuccess = false,
            Message = "Rejected status requires a reason.",
            Data = null,
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
    else
    {
        response = new ResponseDto<object>
        {
            IsSuccess = false,
            Message = "Failed to update business idea status.",
            Data = null,
            StatusCode = StatusCodes.Status500InternalServerError
        };
    }
    return response;
      }
        [HttpGet("ideas-counts")]
        public ResponseDto<BusinessCountsDto> GetBusinessIdeasCounts()
        {
            var counts = _businessService.GetBusinessIdeasCounts();

            return new ResponseDto<BusinessCountsDto>
            {
                IsSuccess = true,
                Message = "Business statistics retrieved successfully.",
                Data = counts,
                StatusCode = StatusCodes.Status200OK
            };
        }

    }
}